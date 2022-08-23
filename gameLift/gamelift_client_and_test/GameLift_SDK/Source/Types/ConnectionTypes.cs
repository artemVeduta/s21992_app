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

namespace Aws.GameLift.Realtime.Types
{
    /// <summary>
    /// Defines the types of connection suites supported by the SDK
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Connect to Realtime server with unsecured websocket and UDP.
        /// This enum value is maintained for legacy purposes and is deprecated in favor of
        /// RT_OVER_WS_UDP_UNSECURED; the two enums have the same value but the latter has a
        /// more descriptive name.
        /// </summary>
        [Obsolete("Use RT_OVER_WS_UDP_UNSECURED instead")]
        RT_OVER_WEBSOCKET = 0,

        /// <summary>
        /// Connect to Realtime server with unsecured websocket and UDP.
        /// </summary>
        RT_OVER_WS_UDP_UNSECURED = 0,

        /// <summary>
        /// Connect to Realtime server with websocket and DTLS secured by TLS 1.2.
        /// </summary>
        RT_OVER_WSS_DTLS_TLS12 = 1
    }
}   
