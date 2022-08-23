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

using Com.Gamelift.Rt.Proto;
using System;
using System.IO;
using System.Threading;
using WebSocket4Net;
using System.Security.Authentication;
using SuperSocket.ClientEngine;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Default websocket connection used in the sdk to communicate with Realtime server over TCP. 
    /// This uses Websocket4Net implementation of websockets.
    /// See: https://github.com/kerryjiang/WebSocket4Net
    /// </summary>
    public class WebSocket4NetWsConnection : WebSocketConnection
    {
        protected AutoResetEvent messageReceiveEvent = new AutoResetEvent(false);
        protected string lastMessageReceived;
        protected WebSocket webSocket;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (messageReceiveEvent != null)
                {
                    messageReceiveEvent.Close();
                    messageReceiveEvent = null;
                }
                if (webSocket != null)
                {
                    webSocket.Close();
                }
            }
        }

        protected override void InitializeWebSocket(string uri)
        {
            webSocket = CreateWebSocket(uri);
            webSocket.Opened += websocket_Opened;
            webSocket.Closed += websocket_Closed;
            webSocket.Error += websocket_Error;
            webSocket.MessageReceived += websocket_MessageReceived;
            webSocket.DataReceived += websocket_DataReceived;
        }

        protected virtual WebSocket CreateWebSocket(string uri)
        {
            // If URI starts with "ws://", client will create a unsecured ws connection.
            // If URI starts with "wss://", client will negotiate with server to create a ws connection secured with 
            // TLS v1.2, other versions will be rejected.
            // See Websocket4Net documentation: https://tiny.amazon.com/1ezl413rl/githkerrWebSblob9a5eWebSWebS
            WebSocket websocket4NetWebSocket = new WebSocket(uri, sslProtocols: SslProtocols.Tls12);

            // If testing with self-signed certs, AllowUnstrustedCertificate needs to be set to true
            websocket4NetWebSocket.Security.AllowUnstrustedCertificate = false;
            websocket4NetWebSocket.Security.AllowNameMismatchCertificate = false;
            websocket4NetWebSocket.Security.AllowCertificateChainErrors = false;

            return websocket4NetWebSocket;
        }

        public override void Open()
        {
            webSocket.Open();

            while (webSocket.State == WebSocketState.Connecting) { Thread.Sleep(50); };   // by default webSocket4Net has AutoSendPing=true,
            // Wait until connection established
            if (webSocket.State != WebSocketState.Open)
            {
                ClientLogger.Error("Unexpected connection state: {0}", webSocket.State);
            }
        }

        protected override void SendData(byte[] data, int len)
        {
            webSocket.Send(data, 0, len);
        }

        public override void Close()
        {
            webSocket.Close();
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            // Signal to listeners
            OnOpen();
        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            // Signal to listeners
            OnError(e.Exception);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            // Signal to listeners
            OnClose();
        }

        // Will be called when server sends a message as a string
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            ClientLogger.Info("Connection - Message received {0}", e.Message);

            lastMessageReceived = e.Message;
            messageReceiveEvent.Set();

            // Assume this is protobuf via json - unexpected delivery path
            Packet packet = Packet.Parser.ParseJson(e.Message);

            // Signal to listeners
            OnPacketReceived(packet);
        }

        // Will be called when server sends a message as bytes
        private void websocket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            byte[] data = e.Data;
            messageReceiveEvent.Set();

            Packet packet;
            using (var stream = new MemoryStream(data))
            {
                packet = Packet.Parser.ParseDelimitedFrom(stream);
            }
            OnPacketReceived(packet);
        }

        /// <summary>
        /// Get security settings of the websocket client. The returned object is a deep copy of the original object.
        /// for defensiveness.
        /// </summary>
        public SecurityOption GetSecurityOption()
        {
            return new SecurityOption()
            {
                EnabledSslProtocols = webSocket.Security.EnabledSslProtocols,
                Certificates = webSocket.Security.Certificates,
                AllowUnstrustedCertificate = webSocket.Security.AllowUnstrustedCertificate,
                AllowCertificateChainErrors = webSocket.Security.AllowCertificateChainErrors,
                AllowNameMismatchCertificate = webSocket.Security.AllowNameMismatchCertificate
            };
        }
    }
}
