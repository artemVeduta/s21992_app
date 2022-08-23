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
    /// Leave group message for player
    /// </summary>
    public class LeaveGroup : RTMessage
    {
        public int GroupId { get; private set; }

        internal LeaveGroup(int sender, int groupId)
            : base(Constants.LEAVE_GROUP_OP_CODE, sender)
        {
            GroupId = groupId;
            TargetPlayer = Constants.PLAYER_ID_SERVER;
        }

        internal override Packet ToInnerPacket(Packet packet)
        {
            var leaveGroup = new Com.Gamelift.Rt.Proto.LeaveGroup
            {
                Group = GroupId
            };

            packet.LeaveGroup = leaveGroup;

            return packet;
        }
    }
}
