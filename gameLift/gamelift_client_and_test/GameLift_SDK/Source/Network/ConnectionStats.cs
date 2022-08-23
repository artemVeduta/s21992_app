/*
* All or portions of this file Copyright (c) Amazon.com, Inc. or its affiliates or
* its licensors.
*
* For complete copyright and license terms please see the LICENSE at the root of this
* distribution (the "License"). All use of this software is governed by the License,
* or, if provided, by the license below or the license accompanying this file. Do not
* remove or modify any license notices. This file is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*/
using System.Threading;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Communication statistics for a single connection
    /// </summary>
    public class ConnectionStats
    {
        // NOTE: may add other stats (e.g. bytes) in the future
        private long messagesSent = 0;
        private long messagesReceived = 0;

        public ConnectionStats GetCopy()
        {
            ConnectionStats stats = new ConnectionStats();
            stats.messagesSent = Interlocked.Read(ref messagesSent);
            stats.messagesReceived = Interlocked.Read(ref messagesReceived);

            return stats;
        }

        public void Reset()
        {
            Interlocked.Exchange(ref messagesSent, 0);
            Interlocked.Exchange(ref messagesReceived, 0);
        }

        public void RecordMessageSent()
        {
            Interlocked.Increment(ref messagesSent);
        }

        public void RecordMessageReceived()
        {
            Interlocked.Increment(ref messagesReceived);
        }

        public long GetMessagesSent()
        {
            return Interlocked.Read(ref messagesSent);
        }

        public long GetMessagesReceived()
        {
            return Interlocked.Read(ref messagesReceived);
        }
    }
}
