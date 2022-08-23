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
namespace Aws.GameLift.Realtime
{
    public class Constants
    {
        // Client caller Version
        public const int CLIENT_VERSION = 2;

        // Player ids
        public const int PLAYER_ID_SERVER = -1;
        public const int PLAYER_ID_CLIENT_INTERNAL = -2;
        // actual players that connect are assigned positive ids

        // Group ids
        public const int GROUP_ID_ALL_PLAYERS = -1;
        // developer-defined ones are all positive

        // Op Codes
        // internal op codes are <= 0   
        public const int NULL_OP_CODE = -99999;

        public const int LOGIN_OP_CODE = 0;
        public const int LOGIN_RESPONSE_OP_CODE = -1;
        public const int PING_RESULT_OP_CODE = -3;
        // UDP connect flow is:
        // client -> server: UDP_CONNECT_OP_CODE
        // server -> client: UDP_CONNECT_SERVER_ACK_OP_CODE (confirms server received initial msg)
        // client -> server: UDP_CONNECT_CLIENT_ACK_OP_CODE (confirms client received server ack)
        public const int UDP_CONNECT_OP_CODE = -5;
        public const int UDP_CONNECT_SERVER_ACK_OP_CODE = -6;
        public const int UDP_CONNECT_CLIENT_ACK_OP_CODE = -7;
        public const int PLAYER_READY_OP_CODE = -8;
        public const int JOIN_GROUP_OP_CODE = -10;
        public const int LEAVE_GROUP_OP_CODE = -11;
        public const int REQUEST_GROUP_MEMBERSHIP_OP_CODE = -12;
        public const int GROUP_MEMBERSHIP_UPDATE_OP_CODE = -13;
        public const int VERIFY_IDENTITY_OP_CODE = -14;
        public const int VERIFY_IDENTITY_RESPONSE_OP_CODE = -15;
        public const int PLAYER_CONNECT_OP_CODE = -101;
        public const int PLAYER_DISCONNECT_OP_CODE = -103;

        // server-initiated messages
        public const int GAME_START_OP_CODE = -1000;
        public const int GAME_END_OP_CODE = -1001;

        // client-initiated messages
        // developer-defined op codes are all > 0
        public const int SEND_MESSAGE_OP_CODE = 1;

        public const int INVALID_PORT = -1;

        // An invalid peerId, typically means player is unconnected to GameLift Realtime
        public const int INVALID_PEER_ID = 0;
        public const int NO_TARGET_PLAYER = 0;
        public const int NO_TARGET_GROUP = 0;
        public const int ZERO_MESSAGE_BYTES = 0;
    }
}