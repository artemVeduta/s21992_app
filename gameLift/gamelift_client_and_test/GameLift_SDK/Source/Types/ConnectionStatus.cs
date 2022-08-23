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
using System;

namespace Aws.GameLift.Realtime.Types
{
    /// <summary>
    /// Defines the status of the Client's connection
    /// </summary>
    public enum ConnectionStatus
    {
        // Client connection is not initialized. 
        READY = -1,
        // Client is trying to connect/reconnect
        CONNECTING,
        // Client is connected over websockets and waiting for action/messages
        CONNECTED,
        // Client is connected overwebsockets and has been validated as able to sent UDP to the server
        CONNECTED_SEND_FAST,
        // Client is connected overwebsockets and has been validated as able to sent UDP to the server
        CONNECTED_SEND_AND_RECEIVE_FAST,
        // Client connection was successfully closed due to an explicit user API call
        DISCONNECTED_CLIENT_CALL,
        // Client connection was closed due to non-user issue
        DISCONNECTED
    }
}
