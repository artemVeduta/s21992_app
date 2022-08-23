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
using System.IO;
using Aws.GameLift.Realtime.Command;
using Com.Gamelift.Rt.Proto;


namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Base class for Websocket implementations (e.g. Websocket4Net, WebGL).
    /// 
    /// TODO: Change message handling to be thread safe / multi event aware
    /// </summary>
    public abstract class WebSocketConnection : BaseConnection
    {
        // TLS connection is disabled by default for backward compatiability reasons, because this feature was not
        // available when RTS was initially released. To create a secured websocket connection, set this value to true.
        public bool TlsEnabled { get; set; } = false;

        public override void Initialize(string hostName, int port)
        {
            Uri webSocketUri = BuildSocketUri(hostName, port);
            ClientLogger.Info("Initializing websocket. Uri: " + webSocketUri);

            InitializeWebSocket(webSocketUri.ToString());
        }

        protected abstract void InitializeWebSocket(string uri);

        public Uri BuildSocketUri(string endpoint, int port)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentException("Cannot construct socket uri: endpoint is undefined");
            }

            if (port <= 0 || port > 65535)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Cannot construct socket uri: port {0} is out of range. Expected 1-65535.", port));
            }

            endpoint = endpoint.Trim();

            string hostName = new UriBuilder(endpoint).Uri.Host.Trim();

            string scheme = TlsEnabled ? "wss" : "ws";

            // Build RTS's websocket address which is in the following formats:
            // ws://hostname/websocket OR wss://hostname/websocket
            return new UriBuilder(scheme, hostName, port, "websocket").Uri;
        }
    }
}
