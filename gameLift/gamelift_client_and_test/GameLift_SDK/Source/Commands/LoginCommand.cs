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

using Packet = Com.Gamelift.Rt.Proto.Packet;

namespace Aws.GameLift.Realtime.Command
{
    /// <summary>
    /// Login command for player
    /// </summary>
    public class LoginCommand : RTMessage
    {
        public string PlayerSessionId { get; private set; }

        internal LoginCommand(string playerSessionId, byte[] payload, int sender)
            : base(Constants.LOGIN_OP_CODE, sender)
        {
            TargetPlayer = Constants.PLAYER_ID_SERVER;
            Payload = payload;
            PlayerSessionId = playerSessionId;
        }

        internal override Packet ToInnerPacket(Packet packet)
        {
            // Set optional login command
            var loginCommand = new Com.Gamelift.Rt.Proto.LoginCommand
            {
                ClientVersion = Constants.CLIENT_VERSION,
            };
            if (PlayerSessionId != null)
            {
                loginCommand.PlayerSessionId = PlayerSessionId;
            }

            packet.Login = loginCommand;

            return packet;
        }
    }
}
