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

using Google.Protobuf;
using Packet = Com.Gamelift.Rt.Proto.Packet;

namespace Aws.GameLift.Realtime.Command
{
    /// <summary>
    /// Message to initiate the UDP handshake process
    /// </summary>
    public class UDPConnectMessage : RTMessage
    {
        internal UDPConnectMessage(int sender)
            : base(Constants.UDP_CONNECT_OP_CODE, sender)
        {
            TargetPlayer = Constants.PLAYER_ID_SERVER;
        }

        internal override Packet ToInnerPacket(Packet packet)
        {
            packet.UdpConnect = new Com.Gamelift.Rt.Proto.UDPConnectMessage();

            return packet;
        }
    }
}
