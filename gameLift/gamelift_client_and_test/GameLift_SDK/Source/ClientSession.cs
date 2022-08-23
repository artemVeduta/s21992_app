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
using System.Collections.Generic;
using System.Linq;

namespace Aws.GameLift.Realtime
{
    /// <summary>
    /// Maintains state about the current player session
    /// 
    /// TODO: Migrate all state information here
    /// </summary>
    public class ClientSession
    {
        public enum SessionState
        {
            READY,          // Client is ready to connect
            OPEN,           // Client has opened a session but is not logged in
            CONNECTED,      // Client has logged in and is ready for messages
            DISCONNECTED,   // Client has disconnected
        }

        public bool LoggedIn { get; set; }
        public ConnectionToken Token {get; private set;}
        public SessionState State { get; set; }
        /// <summary>
        /// Represents the GameLift Realtime peer Id for client. Set on connection.
        /// </summary>
        public virtual int ConnectedPeerId { get; internal set; }
        private HashSet<int> GroupMembership;

        public ClientSession(string endpoint, int port, ConnectionToken token)
        {
            Token = token;
            GroupMembership = new HashSet<int>();
            State = SessionState.READY;
            LoggedIn = false;
        }

        public ClientSession()
        {
            Token = new ConnectionToken();
            GroupMembership = new HashSet<int>();
            State = SessionState.READY;
            LoggedIn = false;
        }

        public virtual void JoinGroup(int groupId)
        {
            GroupMembership.Add(groupId);
        }

        public virtual void LeaveGroup(int groupId)
        {
            GroupMembership.Remove(groupId);
        }

        public void UpdateGroupMembership(int groupId, int[] players)
        {
            if (players.Contains(ConnectedPeerId))
            {
                JoinGroup(groupId);
            }
            else
            {
                LeaveGroup(groupId);
            }
        }

        public bool IsInGroup(int groupId)
        {
            return GroupMembership.Contains(groupId);
        }
    }
}
