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
    /// Group membership update for player. This message is sent from the server to the client.
    /// </summary>
    public class GroupMembershipUpdate : RTMessage
    {
        public int GroupId { get; private set; }
        public int[] PlayerIds { get; private set; }

        internal GroupMembershipUpdate(int sender, int targetPlayer, int groupId, int[] playerIds)
            : base(Constants.GROUP_MEMBERSHIP_UPDATE_OP_CODE, sender)
        {
            GroupId = groupId;
            TargetPlayer = targetPlayer;
            PlayerIds = playerIds;
        }

        public static GroupMembershipUpdate FromProtobuf(Packet packet,
                                                         Com.Gamelift.Rt.Proto.GroupMembershipUpdate message)
        {
            int groupId = Constants.NO_TARGET_GROUP;
            int[] playerIds = null;

            if (message != null)
            {
                groupId = message.Group;
                playerIds = new int[message.Players.Count];
                message.Players.CopyTo(playerIds, 0);
            }

            return new GroupMembershipUpdate(packet.Sender, packet.TargetPlayer, groupId, playerIds);
        }

        internal override Packet ToInnerPacket(Packet packet)
        {
            var groupMembershipUpdate = new Com.Gamelift.Rt.Proto.GroupMembershipUpdate
            {
                Group = GroupId
            };

            groupMembershipUpdate.Players.Add(PlayerIds);

            packet.GroupMembershipUpdate = groupMembershipUpdate;

            return packet;
        }
    }
}
