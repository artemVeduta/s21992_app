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

using System.IO;
using System.Net;
using Com.Gamelift.Rt.Proto;
using static Aws.GameLift.Realtime.Network.MbedDtlsClient;
using System;

namespace Aws.GameLift.Realtime.Network
{
    public class DtlsUdpConnection : UdpConnection
    {
        private MbedDtlsClient dtlsClient;

        private string hostname;
        private byte[] caCert;
        private IPEndPoint remoteEndpoint;
        private IPEndPoint localEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aws.GameLift.Realtime.Network.DtlsUdpConnection"/> class.
        /// </summary>
        /// <param name="listenPort">Listen port</param>
        /// <param name="hostname">The remote host name</param>
        /// <param name="caCert">PEM-encoded certificates (used as trusted CA chain)</param>
        public DtlsUdpConnection(int listenPort, string hostname, byte[] caCert) : base(listenPort)
        {
            this.hostname = hostname;
            this.caCert = caCert;
        }

        public override void Close()
        {
            dtlsClient.Close();
        }

        public override void Open()
        {
            dtlsClient.Open(localEndpoint, remoteEndpoint, hostname, caCert);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dtlsClient != null)
                {
                    dtlsClient.Close();
                }
            }
        }

        protected override void InitializeUdp(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint)
        {
            this.remoteEndpoint = remoteEndpoint;
            this.localEndpoint = localEndpoint;
            dtlsClient = CreateDtlsClient(ReceiveData);
        }

        protected virtual MbedDtlsClient CreateDtlsClient(OnMessage onMessage)
        {
            return new MbedDtlsClient(onMessage);
        }

        private void ReceiveData(byte[] data)
        {
            Packet packet;
            using (var stream = new MemoryStream(data))
            {
                try
                {
                    packet = Packet.Parser.ParseDelimitedFrom(stream);
                }
                catch (Exception e)
                {
                    ClientLogger.Error("Exception parsing packet: " + e.Message);
                    return;
                }
            }
            OnPacketReceived(packet);
        }

        protected override void SendData(byte[] data, int len)
        {
            dtlsClient.Send(data);
        }
    }
}
