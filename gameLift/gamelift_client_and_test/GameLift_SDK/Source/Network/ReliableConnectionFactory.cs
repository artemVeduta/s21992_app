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
    /// Creates instance of WebSocket connection used in the client.
    /// </summary>
    public class ReliableConnectionFactory : ConnectionFactory
    {
        public BaseConnection Create(ConnectionFactoryOptions options)
        {
            ValidateOptions(options);

            ConnectionType connectionType = options.ClientConfiguration.ConnectionType;

            ClientLogger.Info("Creating reliable connection client based on ConnectionType in ClientConfiguration: {0}",
               connectionType);

            switch (connectionType)
            {
                case ConnectionType.RT_OVER_WSS_DTLS_TLS12:
                    ClientLogger.Info("Using Websocket connection secured by TLS 1.2");
                    return new WebSocket4NetWsConnection() { TlsEnabled = true };

                case ConnectionType.RT_OVER_WS_UDP_UNSECURED:
                    ClientLogger.Info("Using unsecured Websocket connection");
                    return new WebSocket4NetWsConnection() { TlsEnabled = false };

                default:
                    ClientLogger.Warn("Unsupported ConnectionType: {0}. Defaulting to Unsecured Websocket connection",
                        connectionType);
                    return new WebSocket4NetWsConnection() { TlsEnabled = false };
            }
        }

        private void ValidateOptions(ConnectionFactoryOptions options)
        {
            if (options.ClientConfiguration == null)
            {
                throw new ArgumentException("Unable to create Reliable connection: ClientConfiguration is undefined.");
            }
        }
    }
}
