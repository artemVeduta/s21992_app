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

namespace Aws.GameLift.Realtime.Event
{
    /// <summary>
    /// Event data for a GroupMembership event (data sent from server) 
    /// </summary>
    public class GroupMembershipEventArgs : BaseEventArgs
    {
        public int GroupId { get; private set; }
        public int[] PlayerIds { get; private set; }

        public GroupMembershipEventArgs(int sender, int groupId, int[] playerIds)
	    : base(sender, Constants.GROUP_MEMBERSHIP_UPDATE_OP_CODE)
        {
            GroupId = groupId;
            PlayerIds = playerIds;
        }
    }
}
