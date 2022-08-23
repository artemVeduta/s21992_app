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
using Aws.GameLift.Realtime.Types;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Creates unsecure/secure UDP connection client based on the 
    /// URL scheme. Url starting with "https://" creates secure client,
    /// otherwise, an unsecure client is created.
    /// 
    /// Both ListenPort and Url are required to be initialized in
    /// ConnectionFactoryOptions.
    /// </summary>
    public class FastConnectionFactory : ConnectionFactory
    {
        public BaseConnection Create(ConnectionFactoryOptions options)
        {
            ValidateOptions(options);

            ConnectionType connectionType = options.ClientConfiguration.ConnectionType;

            ClientLogger.Info("Creating fast connection client based on ConnectionType in ClientConfiguration: {0}",
                connectionType);

            switch(connectionType)
            {
                case ConnectionType.RT_OVER_WSS_DTLS_TLS12:
                    ClientLogger.Info("Using UDP connection secured by DTLS 1.2");
                    return new DtlsUdpConnection(options.UdpListenPort, options.HostName, options.CaCert);

                // This switch case also includes RT_OVER_WEBSOCKET since its has the same value as RT_OVER_WS_UDP_UNSECURED
                case ConnectionType.RT_OVER_WS_UDP_UNSECURED:
                    ClientLogger.Info("Using unsecured UDP connection");
                    return new DotNetUdpConnection(options.UdpListenPort);

                default:
                    ClientLogger.Warn("Unsupported ConnectionType: {0}. Defaulting to Unsecured UDP connection",
                        connectionType);
                    return new DotNetUdpConnection(options.UdpListenPort);
            }
        }

        private void ValidateOptions(ConnectionFactoryOptions options)
        {
            if (options.UdpListenPort < 1 || options.UdpListenPort > 65535)
            {
                throw new ArgumentOutOfRangeException(string.Format("Unable to create fast connection: " +
                       "UDP listen port number is out of range. Expected: 1-65535. Actual: {0}", options.UdpListenPort));
            }

            if (string.IsNullOrWhiteSpace(options.HostName))
            {
                throw new ArgumentException("Unable to create fast connection: host name is undefined");
            }

            if (options.ClientConfiguration == null)
            {
                throw new ArgumentException("Unable to create fast connection: client configuration is undefined");
            }

            // Unsecured fast clients do not need to use CA certificate
            if (options.ClientConfiguration.ConnectionType != ConnectionType.RT_OVER_WS_UDP_UNSECURED &&
                (options.CaCert == null || options.CaCert.Length == 0))
            {
                throw new ArgumentException("Unable to create secured fast connection: CA cert is undefined");
            }
        }
    }
}
