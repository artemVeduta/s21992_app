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
    /// Verify identity command for player
    /// </summary>
    public class VerifyIdentityCommand : RTMessage
    {
        public int PeerId { get; private set; }
        public string ConnectToken { get; private set; }

        internal VerifyIdentityCommand(int peerId, string connectToken, byte[] payload, int sender)
            : base(Constants.VERIFY_IDENTITY_OP_CODE, sender)
        {
            PeerId = peerId;
            ConnectToken = connectToken;
            Payload = payload;
        }

        internal override Packet ToInnerPacket(Packet packet)
        {
            // Set optional verify identity command
            var verifyIdentityCommand = new Com.Gamelift.Rt.Proto.VerifyIdentityCommand
            {
                PeerId = PeerId,
                ConnectToken = ConnectToken,
            };

            packet.VerifyIdentity = verifyIdentityCommand;

            return packet;
        }
    }
}
