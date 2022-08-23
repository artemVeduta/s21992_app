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

namespace Aws.GameLift.Realtime.Command
{
    /// <summary>
    /// Verify identity result from GameLift Realtime
    /// </summary>
    public class VerifyIdentityResult : RTMessage
    {
        public bool Success { get; private set; }

        internal VerifyIdentityResult(int peerId, int sender, bool success, byte[] payload)
            : base(Constants.VERIFY_IDENTITY_RESPONSE_OP_CODE, sender)
        {
            TargetPlayer = peerId;
            Payload = payload;
            Success = success;
        }

        public static VerifyIdentityResult FromProtobuf(int sender, byte[] payload, Com.Gamelift.Rt.Proto.VerifyIdentityResult message)
        {
            bool success = false;
            int peerId = Constants.INVALID_PEER_ID;

            if (message != null)
            {
                peerId = message.PeerId;
                success = message.Success;
            }

            return new VerifyIdentityResult(peerId, sender, success, payload);
        }
    }
}
