/*
* All or portions of this file Copyright (c) Amazon.com, Inc. or its affiliates or
* its licensors.
*
* For complete copyright and license terms please see the LICENSE at the root of this
* distribution (the "License"). All use of this software is governed by the License,
* or, if provided, by the license below or the license accompanying this file. Do not
* remove or modify any license notices. This file is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Com.Gamelift.Rt.Proto;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Standard (unsecured) UDP implementation used in the sdk.
    /// </summary>
    public class DotNetUdpConnection : UdpConnection
    {
        private UdpClient udp;
        private IPEndPoint remoteEndpoint;

        public DotNetUdpConnection(int listenPort) : base(listenPort)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (udp != null)
                {
                    udp.Close();
                }
            }
        }

        protected override void InitializeUdp(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint)
        {
            this.remoteEndpoint = remoteEndpoint;

            udp = CreateUdpClient(localEndpoint);

            udp.BeginReceive(new AsyncCallback(ReceiveData), this);
        }

        protected virtual UdpClient CreateUdpClient(IPEndPoint ipEndPoint)
        {
            return new UdpClient(ipEndPoint);
        }

        private void ReceiveData(IAsyncResult result)
        {
            try
            {
                byte[] data = udp.EndReceive(result, ref remoteEndpoint);

                try
                {
                    Packet packet;
                    using (var stream = new MemoryStream(data))
                    {
                        packet = Packet.Parser.ParseDelimitedFrom(stream);
                    }
                    OnPacketReceived(packet);
                }
                finally
                {
                    udp.BeginReceive(new AsyncCallback(ReceiveData), this);
                }
            } catch (ObjectDisposedException e)
            {
                // This exception is thrown if we have received data after the connection
                // is closed for whatever reason. We can safely ignore it and continue.
                return;
            }
        }

        protected override void SendData(byte[] data, int len)
        {
            udp.Send(data, data.Length, remoteEndpoint);
        }

        public override void Open()
        {
        }

        public override void Close()
        {
            udp.Close();
        }
    }
}
