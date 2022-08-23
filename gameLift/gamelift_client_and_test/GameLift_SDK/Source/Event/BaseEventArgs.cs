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

namespace Aws.GameLift.Realtime.Event
{
    /// <summary>
    /// Event data for a generic event.  All classes should derive from this.
    /// </summary>
    public abstract class BaseEventArgs : EventArgs
    {
        public int Sender { get; private set; }
        public int OpCode { get; private set; }

        // opaque pass-through data defined by the developer
        public byte[] Data { get; private set; }

        protected BaseEventArgs(int sender, int opCode) : this(sender, opCode, null)
        {
        }

        protected BaseEventArgs(int sender, int opCode, byte[] data)
        {
            Sender = sender;
            OpCode = opCode;
            Data = data;
        }
    }
}
