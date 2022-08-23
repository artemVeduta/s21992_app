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
using Aws.GameLift.Realtime.Network;

namespace Aws.GameLift.Realtime
{
    /// <summary>
    /// Base functionality for all connection types used to talk to GameLift Realtime Servers
    /// </summary>
    public interface Connection
    {
        /// <summary>
        /// 
        /// </summary>
        void Initialize(string hostName, int port);
        /// <summary>
        /// 
        /// </summary>
        void Open();
        /// <summary>
        /// 
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        void Terminate();

        /// <summary>
        /// Send a request to a Lightweight Server
        /// </summary>
        /// <param name="request">The request to send </param> 
        /// <returns>The number of bytes sent</returns>
        int Send(Com.Gamelift.Rt.Proto.Packet packet);

        /// <summary>
        /// Handle as response from a Lightweight Servers
        /// </summary>
        /// <param name="packet">The packet recieved</param>
        void OnPacketReceived(Com.Gamelift.Rt.Proto.Packet packet);

        /// <summary>
        /// Return accumulated statistics on this connection
        /// </summary>
        ConnectionStats GetStats();

        /// <summary>
        /// Resets accumulated statistics on this connection to zeroes
        /// </summary>
        void ResetStats();
    }
}
