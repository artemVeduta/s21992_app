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
using System;
using Aws.GameLift.Realtime.Event;
using ErrorEventArgs = Aws.GameLift.Realtime.Event.ErrorEventArgs;

namespace Aws.GameLift.Realtime
{
    /// <summary>
    /// Common event handler for Client events
    /// </summary>
    public class ClientEvents
    {
        // Connnection Events
        public event EventHandler ConnectionOpen;
        public event EventHandler ConnectionClose;
        public event EventHandler<ErrorEventArgs> ConnectionError;

        // Communication Events
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<GroupMembershipEventArgs> GroupMembershipUpdated;

        public void OnOpen()
        {
            ConnectionOpen?.Invoke(this, null);
        }

        public void OnClose()
        {
            ConnectionClose?.Invoke(this, null);
        }

        public void OnError(Exception e)
        {
            ConnectionError?.Invoke(this, new ErrorEventArgs(e));
        }

        public void OnDataReceived(int sender, int opCode, byte[] data)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArgs(sender, opCode, data));
        }

        public void OnGroupMembershipUpdated(GroupMembershipEventArgs groupMembershipEventArgs)
        {
            GroupMembershipUpdated?.Invoke(this, groupMembershipEventArgs);
        }
    }
}
