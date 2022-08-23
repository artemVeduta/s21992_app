/*
* All or portions of this file Copyright (c) Amazon.com, Inc. or its affiliates or
* its licensors.
*
* For complete copyright and license terms please see the LICENSE at the root of this
* distribution (the "License"). All use of this software is governed by the License,
* or, if provided, by the license below or the license accompanying this file. Do not
* remove or modify any license notices. This file is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*
*/

using System;
using System.Net;
using System.Net.Sockets;
using GameLiftRealtimeNative;

namespace Aws.GameLift.Realtime.Network
{
    public class MbedDtlsClient
    {
        public delegate void OnMessage(byte[] data);

        private UdpClient udp;
        private DTLSConnection dtls;
        private IPEndPoint remoteEndPoint;
        private Callback sendEncryptedCallback;
        private readonly OnMessage onMessage;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="onMessage">Callback invoked when received data from the remote</param>
        public MbedDtlsClient(OnMessage onMessage)
        {
            this.onMessage = onMessage;
        }

        /// <summary>
        /// Performs DTLS handshake.
        /// </summary>
        /// <param name="localEndPoint">The local endpoint the underlying UDP client is listening to</param>
        /// <param name="remoteEndPoint">The remote endpoint to send UDP packets to</param>
        /// <param name="hostname">The remote host name</param>
        /// <param name="certificates">PEM-encoded certificates (used as trusted CA chain)</param>
        public void Open(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, string hostname, byte[] caCert)
        {
            udp = new UdpClient(localEndPoint);
            udp.BeginReceive(new AsyncCallback(ReceiveData), this);
            this.remoteEndPoint = remoteEndPoint;
            // store callback as instance variable to avoid Garbage Collection
            this.sendEncryptedCallback = new Callback(udp, this.remoteEndPoint);
            dtls = new DTLSConnection(sendEncryptedCallback);
            // size needs to include the final NULL for PEM-encoded certificates
            string caCertString = System.Text.Encoding.UTF8.GetString(caCert, 0, caCert.Length);
            DTLSConnection.Result result = dtls.open(hostname, caCertString, (uint)caCertString.Length + 1);

            if (result.get_type() != DTLSConnection.Result.Type.Open)
            {
                throw new Exception(result.get_info());
            }
        }

        /// <summary>
        /// Send data to the remote endpoint
        /// </summary>
        /// <param name="data">data to send</param>
        public virtual void Send(byte[] data)
        {
            NativeByteArray nativeArray = NativeHelper.ToNativeByteArray(data);
            dtls.send(nativeArray.cast(), (uint) data.Length);
        }

        private void ReceiveData(IAsyncResult result)
        {
            try
            {
                byte[] data = udp.EndReceive(result, ref remoteEndPoint);
                NativeByteArray nativeArray = NativeHelper.ToNativeByteArray(data);
                DTLSConnection.Result decrypted = dtls.receive_message(nativeArray.cast(), (uint)data.Length);
                if (decrypted.get_type() == DTLSConnection.Result.Type.Message)
                {
                    onMessage.Invoke(NativeHelper.FromNativeByteArrayPointer(decrypted.get_message(), decrypted.get_length()));
                }
                udp.BeginReceive(new AsyncCallback(ReceiveData), this);
            }
            catch (ObjectDisposedException e)
            {
                // This exception is thrown if we have received data after the connection
                // is closed for whatever reason. We can safely ignore it and continue.
                return;
            }
        }

        public void Close()
        {
            try
            {
                dtls.close();
            }
            catch (Exception e)
            {
                ClientLogger.Error("Exception occurred while closing dtls connection: {0}", e.Message);
            }

            try
            {
                udp.Close();
            }
            catch (Exception e)
            {
                ClientLogger.Error("Exception occurred while closing dtls socket: {0}", e.Message);
            }
        }
    }

    /// <summary>
    /// Class for enabling callback from native code.
    /// Reference: http://www.swig.org/Doc4.0/CSharp.html#CSharp_directors
    /// </summary>
    class Callback : SendEncryptedCallback
    {
        private readonly UdpClient udp;
        private readonly IPEndPoint remoteEndpoint;

        public Callback(UdpClient udp, IPEndPoint remoteEndpoint)
        {
            this.udp = udp;
            this.remoteEndpoint = remoteEndpoint;
        }

        public override void sendEncrypted(SWIGTYPE_p_unsigned_char buf, uint len)
        {
            byte[] data = NativeHelper.FromNativeByteArrayPointer(buf, len);
            udp.Send(data, data.Length, remoteEndpoint);
        }
    }
}
