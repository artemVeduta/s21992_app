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

using GameLiftRealtimeNative;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Utility class for working with native strctures.
    /// </summary>
    public static class NativeHelper
    {
        /// <summary>
        /// Creates a NativeByteArray from a byte[]
        ///
        /// Note: not returning the SWIG wrapper because the lifecycle of the native array is tied to the NativeByteArray object
        /// </summary>
        /// <param name="data">the source byte array</param>
        /// <returns>the NativeByteArray created</returns>
        public static NativeByteArray ToNativeByteArray(byte[] data)
        {
            NativeByteArray array = new NativeByteArray(data.Length);
            for (int i = 0; i < data.Length; ++i)
            {
                array.setitem(i, data[i]);
            }
            return array;
        }

        /// <summary>
        /// Creates a byte[] from a native unsiged char pointer
        /// </summary>
        /// <param name="pointer">the SWIG wrapper for the native unsigned char pointer</param>
        /// <param name="length">the length of the array</param>
        /// <returns>the byte[] created</returns>
        public static byte[] FromNativeByteArrayPointer(SWIGTYPE_p_unsigned_char pointer, uint length)
        {
            NativeByteArray array = NativeByteArray.frompointer(pointer);
            byte[] data = new byte[length];
            for (int i = 0; i < length; ++i)
            {
                data[i] = array.getitem(i);
            }
            return data;
        }
    }
}
