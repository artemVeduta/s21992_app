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
using Aws.GameLift.Realtime.Network;
using Aws.GameLift.Realtime.Types;

namespace Aws.GameLift.Realtime
{
    /// <summary>
    /// Defines connection properties to use when talking to GameLift Realtime Servers.
    /// </summary>
    public class ClientConfiguration
    {
        /// <summary>
        /// Type of connection suite to use to connect to GameLift Realtime Servers.
        /// </summary>
        /// <value>The type of the connection.</value>
        public ConnectionType ConnectionType { get; set; } = ConnectionType.RT_OVER_WS_UDP_UNSECURED;

        /// <summary>
        /// Provides implementation of reliable connections based on the ConnectionType.
        /// </summary>
                                                                   /// <value>The reliable connection factory.</value>
        public ConnectionFactory ReliableConnectionFactory { get; set; } = new ReliableConnectionFactory();

        /// <summary>
        /// Provides implementation of fast connections based on the ConnectionType.
        /// </summary>
        /// <value>The fast connection factory.</value>
        public ConnectionFactory FastConnectionFactory { get; set; } = new FastConnectionFactory();

        /// <summary>
        /// Create a default ClientConfiguration, which contains the following configuration for the game client:
        /// - Connects to Gamelift Realtime Server
        /// - Establishes reliable connection to server with Websocket4Net implementation provided by Gamelift
        /// - Establishes fast connection to server with .NET UDP implementation provided by Gamelift
        /// - Reliable connection is Unsecured
        /// - Fast connection is Unsecured
        /// </summary>
        /// <returns>The default game client configuration</returns>
        [Obsolete("This connection is unsecured! Prefer a secured ConnectionType instead! Note, secure connection " +
            "establishment requires the Realtime server to contain generated certificates. Refer to AWS Gamelift " +
            "documentations for creating a certificate-enabled Script fleet.")]
        public static ClientConfiguration Default()
        {
            return new ClientConfiguration();
        }
    }
}
