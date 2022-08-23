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

namespace Aws.GameLift.Realtime.Command
{
    /// <summary>
    /// Login result from GameLift Realtime
    /// </summary>
    public class LoginResult : RTMessage
    {
        public string ConnectToken { get; private set; }
        public string ReconnectToken { get; private set; }
        public bool Success { get; private set; }
        public int UdpPort { get; private set; }
        public byte[] CaCert { get; private set; }

        internal LoginResult(int peerId, int sender, bool success, string connectToken, string reconnectToken, int udpPort, byte[] caCert, byte[] payload)
            : base(Constants.LOGIN_RESPONSE_OP_CODE, sender)
        {
            ConnectToken = connectToken;
            ReconnectToken = reconnectToken;
            Success = success;
            TargetPlayer = peerId;
            Payload = payload;
            UdpPort = udpPort;
            CaCert = caCert;
        }

        public static LoginResult FromProtobuf(int sender, byte[] payload, Com.Gamelift.Rt.Proto.LoginResult message)
        {
            bool success = false;
            int peerId = Constants.INVALID_PEER_ID;
            int udpPort = Constants.INVALID_PORT;
            byte[] caCert = null;
            string reconnectToken = null;
            string connectToken = null;

            if (message != null)
            {
                success = message.Success;
                peerId = message.PeerId;
                reconnectToken = message.ReconnectToken;
                udpPort = message.FastPort;
                caCert = message.CaCert?.ToByteArray();
                connectToken = message.ConnectToken;
            }

            return new LoginResult(peerId, sender, success, connectToken, reconnectToken, udpPort, caCert, payload);
        }
    }
}
