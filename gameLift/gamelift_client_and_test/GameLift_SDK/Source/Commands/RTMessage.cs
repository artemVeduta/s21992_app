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
using Aws.GameLift.Realtime.Types;
using Com.Gamelift.Rt.Proto;
using Google.Protobuf;

namespace Aws.GameLift.Realtime.Command
{
    /// <summary>
    /// Base class for all communication between the RT server and client.
    /// </summary>
    public class RTMessage
    {
        public int OpCode { get; protected set; }
        public int Sender { get; private set; }
        public int TargetPlayer { get; protected set; }
        public int TargetGroup { get; protected set; }
        public DeliveryIntent DeliveryIntent { get; protected set; }
        public byte[] Payload { get; protected set; }

        internal RTMessage(int opCode, int sender)
        {
            OpCode = opCode;
            Sender = sender;
            TargetPlayer = Constants.NO_TARGET_PLAYER;
            TargetGroup = Constants.NO_TARGET_GROUP;
            DeliveryIntent = DeliveryIntent.Fast; // default to udp
            Payload = null;
        }

        public RTMessage WithTargetPlayer(int targetPlayer)
        {
            TargetPlayer = targetPlayer;

            return this;
        }

        public RTMessage WithTargetGroup(int targetGroup)
        {
            TargetGroup = targetGroup;

            return this;
        }

        public RTMessage WithPayload(byte[] payload)
        {
            Payload = payload;

            return this;
        }

        public RTMessage WithDeliveryIntent(DeliveryIntent deliveryIntent)
        {
            DeliveryIntent = deliveryIntent;

            return this;
        }
        /// <summary>
        /// Takes the given outer packet and merges it with the specific inner message
        /// </summary>
        /// <param name="packet">The base packet to merge with</param>
        /// <returns>The final packet ready for transmission</returns>
        internal virtual Packet ToInnerPacket(Packet packet)
        {
            return packet;
        }

        /// <summary>
        /// Wrap base information into packet. 
        /// </summary>
        /// <returns></returns>
        internal Packet ToPacket()
        {
            Packet packet = new Packet
            {
                OpCode = OpCode,
                Sender = Sender
            };

            if (TargetPlayer != Constants.NO_TARGET_PLAYER)
            {
                packet.TargetPlayer = TargetPlayer;
            }

            if (TargetGroup != Constants.NO_TARGET_GROUP)
            {
                packet.TargetGroup = TargetGroup;
            }

            if (Payload != null && Payload.Length > 0)
            {
                packet.Payload = ByteString.CopyFrom(Payload);
            }

            packet.Reliable = (DeliveryIntent == DeliveryIntent.Reliable);

            return ToInnerPacket(packet);
        }

        internal static RTMessage FromPacket(Packet packet)
        {
            RTMessage message = null;

            byte[] payload = null;
            if (packet.Payload != null) {
                payload = packet.Payload.ToByteArray();
            }
            switch (packet.OpCode)
            {
                case Constants.LOGIN_RESPONSE_OP_CODE:
                    message = LoginResult.FromProtobuf(packet.Sender,
                                                       payload,
                                                       packet.LoginResult);
                    break;

                case Constants.GROUP_MEMBERSHIP_UPDATE_OP_CODE:
                    message = GroupMembershipUpdate.FromProtobuf(packet, packet.GroupMembershipUpdate);
                    break;

                case Constants.VERIFY_IDENTITY_RESPONSE_OP_CODE:
                    message = VerifyIdentityResult.FromProtobuf(packet.Sender, payload, packet.VerifyIdentityResult);
                    break;

                default:
                    DeliveryIntent deliveryIntent = packet.Reliable ? DeliveryIntent.Reliable : DeliveryIntent.Fast;
                    return new RTMessage(packet.OpCode, packet.Sender)
                        .WithTargetPlayer(packet.TargetPlayer)
                        .WithPayload(payload)
                        .WithDeliveryIntent(deliveryIntent);
            }

            return message;
        }
    }
}
