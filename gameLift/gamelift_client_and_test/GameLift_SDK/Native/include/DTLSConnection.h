#ifndef DTLSCONNECTION_H
#define DTLSCONNECTION_H

#define MBEDTLS_PEM_PARSE_C

#include <mutex>
#include <string>
#include "mbedtls/config.h"
#include "mbedtls/net.h"
#include "mbedtls/debug.h"
#include "mbedtls/ssl.h"
#include "mbedtls/entropy.h"
#include "mbedtls/ctr_drbg.h"
#include "mbedtls/error.h"
#include "mbedtls/certs.h"
#include "mbedtls/timing.h"

namespace Aws {
    namespace GameLift {
        /**
         * Base callback class for send encrypted data.
         * Reference: http://www.swig.org/Doc4.0/CSharp.html#CSharp_directors
         */
        class SendEncryptedCallback {
        public:
            virtual ~SendEncryptedCallback() {}
            virtual void sendEncrypted(const unsigned char *buf, size_t len) = 0;
        };

        /**
         * Class for managing DTLS. It does not manage a UDP socket by itself, so an external UDP socket/UdpClient is required.
         */
        class DTLSConnection {
        public:

            class Result {
            public:
                enum class Type {
                    Close, Open, Error, Message, OK
                };

                Result(const Result& result):
                    type(result.type), length(result.length), info(result.info), code(result.code)
                {
                    message = new unsigned char[length];
                    memcpy(message, result.message, length);
                }
                Result(Result&& result):
                    type(result.type), message(result.message), info(result.info), code(result.code)
                {
                    result.message = nullptr;
                }
                ~Result()
                {
                    delete [] message;
                }

                Type get_type() const { return type; }
                unsigned char* get_message() const { return message; }
                size_t get_length() const { return length; }
                std::string get_info() const { return info; }
                int get_code() const { return code; }
            private:
                friend class DTLSConnection;
                Result(unsigned char *msg, size_t length)
                    : type(Type::Message), length(length), info(""), code(0)
                {
                    message = new unsigned char[length];
                    memcpy(message, msg, length);
                }
                Result(Type type, std::string info, int code): type(type), message(nullptr), length(0), info(info), code(code) {}

                Type type;
                unsigned char *message;
                size_t length;
                std::string info;
                int code;
            };

            DTLSConnection(SendEncryptedCallback *callback);
            virtual ~DTLSConnection();

            /**
             * Performs DTLS handshake
             * @param host the name of the remote host
             * @param trusted_cert certificates
             * @param trusted_cert_len the length of the trusted_cert
             * @return the result of the handshake
             */
            Result open(std::string host, char* trusted_cert, size_t trusted_cert_len);

            /**
             * Sends message to remote host. SendEncryptedCallback is used during the sending.
             * @param message the message to send
             * @param len the length of the message
             */
            Result send(const unsigned char *message, size_t len);

            /**
             * Process the messages received from the external UDP socket
             * @param message the received encrypted message
             * @param len the length of the message
             * @return the result containing the decrypted message
             */
            Result receive_message(const unsigned char *message, size_t len);

            /**
             * Close the DTLS connection
             */
            Result close();
        private:
            int send_encrypted(const unsigned char *buf, size_t len);
            int recv_encrypted(unsigned char *buf, size_t len);

            // BIO callback functions supplied to mbedtls
            static int net_send(void *ctx, const unsigned char *buf, size_t len);
            static int net_recv(void *ctx, unsigned char *buf, size_t len);

            mbedtls_net_context server_fd;
            mbedtls_x509_crt cacert;
            mbedtls_ssl_context ssl;
            mbedtls_ssl_config conf;
            mbedtls_ctr_drbg_context ctr_drbg;
            mbedtls_entropy_context entropy;
            mbedtls_timing_delay_context timer;
            SendEncryptedCallback *send_callback;

            const unsigned char* buffer;
            int buffer_len;

            std::mutex mutex;
        };
    };
};

#endif /* DTLSConnection_h */
