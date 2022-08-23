#include "DTLSConnection.h"
#include <cstdio>
#include <cstdlib>

using namespace Aws::GameLift;

static void my_debug(void *ctx, int level,
                     const char *file, int line,
                     const char *str)
{
    ((void) level);

    fprintf((FILE *) ctx, "%s:%04d: %s", file, line, str);
    fflush((FILE *) ctx);
}

DTLSConnection::DTLSConnection(SendEncryptedCallback *callback):
    buffer(NULL),
    buffer_len(0),
    send_callback(callback)
{
    mbedtls_net_init(&server_fd);
    mbedtls_ssl_init(&ssl);
    mbedtls_ssl_config_init(&conf);
    mbedtls_x509_crt_init(&cacert);
    mbedtls_ctr_drbg_init(&ctr_drbg);
    mbedtls_entropy_init(&entropy);
}

DTLSConnection::~DTLSConnection()
{
    mbedtls_net_free(&server_fd);
    mbedtls_ssl_free(&ssl);
    mbedtls_ssl_config_free(&conf);
    mbedtls_x509_crt_free(&cacert);
    mbedtls_ctr_drbg_free(&ctr_drbg);
    mbedtls_entropy_free(&entropy);
}

DTLSConnection::Result DTLSConnection::open(std::string host, char *trusted_cert, size_t trusted_cert_len)
{
    std::lock_guard<std::mutex> lock(mutex);

    mbedtls_debug_set_threshold(0);

    int ret = 0;
    const char *pers = "dtls_client";

    if ((ret = mbedtls_ctr_drbg_seed(&ctr_drbg,
                                     mbedtls_entropy_func,
                                     &entropy,
                                     reinterpret_cast<const unsigned char *>(pers),
                                     strlen(pers))) != 0)
    {
        return {Result::Type::Error, "mbedtls_ctr_drbg_seed", ret};
    }

    if ((ret = mbedtls_x509_crt_parse(&cacert,
                                      reinterpret_cast<const unsigned char *>(trusted_cert),
                                      trusted_cert_len)) != 0)
    {
        return {Result::Type::Error, "mbedtls_x509_crt_parse", ret};
    }

    if ((ret = mbedtls_ssl_config_defaults(&conf,
                                           MBEDTLS_SSL_IS_CLIENT,
                                           MBEDTLS_SSL_TRANSPORT_DATAGRAM,
                                           MBEDTLS_SSL_PRESET_DEFAULT)) != 0)
    {
        return {Result::Type::Error, "mbedtls_ssl_config_defaults", ret};
    }

    mbedtls_ssl_conf_authmode(&conf, MBEDTLS_SSL_VERIFY_REQUIRED);
    mbedtls_ssl_conf_ca_chain(&conf, &cacert, NULL);
    mbedtls_ssl_conf_rng(&conf, mbedtls_ctr_drbg_random, &ctr_drbg);
    mbedtls_ssl_conf_dbg(&conf, my_debug, stdout);

    if ((ret = mbedtls_ssl_setup(&ssl, &conf)) != 0)
    {
        return {Result::Type::Error, "mbedtls_ssl_setup", ret};
    }

    if ((ret = mbedtls_ssl_set_hostname(&ssl, host.c_str())) != 0)
    {
        return {Result::Type::Error, "mbedtls_ssl_set_hostname", ret};
    }

    mbedtls_ssl_set_bio(&ssl, this, net_send, net_recv, NULL);

    mbedtls_ssl_set_timer_cb(&ssl, &timer, mbedtls_timing_set_delay,
                             mbedtls_timing_get_delay);

    do
    {
        ret = mbedtls_ssl_handshake(&ssl);
    } while (ret == MBEDTLS_ERR_SSL_WANT_READ || ret == MBEDTLS_ERR_SSL_WANT_WRITE);

    if (ret != 0)
    {
        return {Result::Type::Error, "mbedtls_ssl_handshake", ret};
    }

    mbedtls_net_set_nonblock(&server_fd);

    return {Result::Type::Open, "", ret};
}

int DTLSConnection::send_encrypted(const unsigned char *buf, size_t len)
{
    send_callback->sendEncrypted(buf, len);
    return len;
}

DTLSConnection::Result DTLSConnection::send(const unsigned char *message, size_t len)
{
    int ret = 0;

    do
    {
        ret = mbedtls_ssl_write(&ssl, message, len);
    } while (ret == MBEDTLS_ERR_SSL_WANT_READ || ret == MBEDTLS_ERR_SSL_WANT_WRITE);

    if (ret < 0)
    {
        return {Result::Type::Error, "mbedtls_ssl_write", ret};
    }

    return {Result::Type::OK, "", 0};
}

DTLSConnection::Result DTLSConnection::receive_message(const unsigned char *message, size_t length)
{
    // store the message so it can be accessed by mbedtls (in net_recv)
    buffer = message;
    buffer_len = length;

    // still doing handshake, message doesn't need to be decrypted
    if (ssl.state != MBEDTLS_SSL_HANDSHAKE_OVER)
    {
        return {Result::Type::OK, "", 0};
    }

    // comfortably above our max supported message size
    // (at time of writing, 1200; See Aws.GameLift.Realtime.Client.maxFastMessageBytes)
    const int bufLen = 2048;
    unsigned char buf[bufLen];
    memset(buf, 0, bufLen);
    int len = mbedtls_ssl_read(&ssl, buf, bufLen);

    if (len > 0)
    {
        return {buf, static_cast<size_t>(len)};
    }
    else if (len == MBEDTLS_ERR_SSL_PEER_CLOSE_NOTIFY)
    {
        return close();
    }
    else
    {
        return {Result::Type::Error, "Unexpected return code while receiving message", len};
    }
}

DTLSConnection::Result DTLSConnection::close()
{
    if (ssl.state != MBEDTLS_SSL_HANDSHAKE_OVER)
    {
        return {Result::Type::Error, "Attempted to close unestablishe SSL connection", 0};
    }

    int ret = 0;
    do
    {
        ret = mbedtls_ssl_close_notify( &ssl );
    } while (ret == MBEDTLS_ERR_SSL_WANT_WRITE);

    return {Result::Type::Close, "", ret};
}

int DTLSConnection::recv_encrypted(unsigned char *buf, size_t len)
{
    if (buffer_len != 0) {
        len = buffer_len;
        memcpy(buf, buffer, buffer_len);
        buffer = NULL;
        buffer_len = 0;
        return len;
    }

    return MBEDTLS_ERR_SSL_WANT_READ;
}

int DTLSConnection::net_send(void *ctx, const unsigned char *buf, size_t len)
{
    DTLSConnection *dtls = (DTLSConnection *) ctx;
    return dtls->send_encrypted(buf, len);
}

int DTLSConnection::net_recv(void *ctx, unsigned char *buf, size_t len)
{
    DTLSConnection *dtls = (DTLSConnection *) ctx;
    return dtls->recv_encrypted(buf, len);
}
