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
using Aws.GameLift.Realtime.Command;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Common event handler for Client events
    /// </summary>
    public class NetworkEvents
    {
        // Connnection Events
        public event EventHandler ConnectionOpen;
        public event EventHandler ConnectionClose;
        public event EventHandler<ErrorEventArgs> ConnectionError;

        // Communication Events
        public event EventHandler<MessageEventArgs> MessageReceived;

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

        public void OnMessageReceived(RTMessage result)
        {
            MessageReceived?.Invoke(this, new MessageEventArgs(result));
        }
    }

    /// <summary>
    /// Event data for a ResponseEvent event (data sent from server) 
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        // opaque pass-through data defined by the developer
        public RTMessage Result { get; private set; }

        public MessageEventArgs(RTMessage result)
        {
            Result = result;
        }
    }
}
