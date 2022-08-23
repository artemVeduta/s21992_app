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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Com.Gamelift.Rt.Proto;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Base class for UDP connection implementations.
    /// </summary>
    public abstract class UdpConnection : BaseConnection
    {
        protected int listenPort;

        // For validating packet order (older packets are dropped)
        private object sequenceLock = new object();
        private int clientSequence = -1;
        private int serverSequence = -1;

        public UdpConnection(int listenPort)
        {
            this.listenPort = listenPort;
        }

        public override void Initialize(string hostName, int remotePort)
        {
            ClientLogger.Info("Initializing udp. Url: {0}, Remote Port: {1}, Listen Port: {2}",
                hostName, remotePort, listenPort);

            hostName = RemoveSchemeIfExists(hostName);

            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            if (addresses.Length <= 0)
            {
                throw new Exception("Unable to lookup remote host: " + hostName);
            }
            else if (addresses.Length > 1)
            {
                // It's possible for addresses to have multiple values if the hostname is mapped to both IPv6 and IPv4,
                // In those cases, IPv6 will take precendence over IPv4 address.
                List<string> addressStrings = new List<string>();
                foreach (IPAddress address in addresses)
                {
                    addressStrings.Add(address.ToString());
                }

                ClientLogger.Info("Multiple IP addresses: [{0}] found for hostname: {1}. Selecting the first result: {2}.",
                    string.Join(",", addressStrings), hostName, addresses[0]);
            }

            IPAddress remoteAddress = addresses[0];
            IPAddress localAddress =
                (remoteAddress.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any;

            IPEndPoint remoteEndpoint = new IPEndPoint(remoteAddress, remotePort);
            IPEndPoint localEndpoint = new IPEndPoint(localAddress, listenPort);

            ClientLogger.Info("Establishing UDP connection between server endpoint: {0} and local endpoint: {1}",
                remoteEndpoint.ToString(), localEndpoint.ToString());

            InitializeUdp(localEndpoint, remoteEndpoint);
        }

        private static string RemoveSchemeIfExists(string uri)
        {
            return new UriBuilder(uri).Uri.Host;
        }

        protected abstract void InitializeUdp(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint);

        protected override void BeforeSend(Packet packet)
        {
            lock (sequenceLock)
            {
                packet.SequenceNumber = ++clientSequence;
            }
        }

        protected override bool CanReceive(Packet packet)
        {
            lock (sequenceLock)
            {
                if (packet.SequenceNumber <= serverSequence)
                {
                    // Old packet -- drop it
                    return false;
                }

                serverSequence = packet.SequenceNumber;
            }

            return true;
        }
    }
}
