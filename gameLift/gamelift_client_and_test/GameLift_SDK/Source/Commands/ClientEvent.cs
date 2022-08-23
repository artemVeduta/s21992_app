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
    /// Send a developed defined client event to the server.
    /// </summary>
    public class ClientEvent : RTMessage
    {
        /// <summary>
        /// OpCode should be postive number that is defined by game 
        /// </summary>
        /// <param name="opCode">The opCode of the event </param>
        /// <param name="sender">The sender, player or known game client id</param>
        public ClientEvent(int opCode, int sender)
            : base(opCode, sender)
        {
            TargetPlayer = Constants.PLAYER_ID_SERVER;
        }

        public ClientEvent(int opCode, int sender, byte[] payload)
            : base(opCode, sender)
        {
            TargetPlayer = Constants.PLAYER_ID_SERVER;
            Payload = payload;
        }

        internal override Packet ToInnerPacket(Packet packet)
        {
            return packet;
        }
    }
}
