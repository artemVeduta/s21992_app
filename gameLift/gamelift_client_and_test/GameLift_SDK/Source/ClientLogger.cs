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
using System.Text;

namespace Aws.GameLift.Realtime
{
    /// <summary>
    /// Simple logger for GameLift Realtime Client SDK
    /// </summary>
    public static class ClientLogger
    {
        public enum LogLevel
        {
            DEBUG,
            INFO,
            ERROR,
            WARN
        }

        public static string LOG_FORMAT_STRING = "[{0}] {1}";

        public static void Log(LogLevel level, string message)
        {
            Output(level, message);
        }

        public static void Info(string message, params object[] args)
        {
            Log(LogLevel.INFO, message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            Log(LogLevel.WARN, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Log(LogLevel.ERROR, message, args);
        }

        private static void Log(LogLevel level, string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = String.Format(message, args);
            }
            Output(level, message);
        }

        private static void Output(LogLevel level, string message)
        {
            message = String.Format(LOG_FORMAT_STRING, level.ToString(), message);
            if (LogHandler != null)
            {
                LogHandler(message);
            }
            System.Diagnostics.Debug.WriteLine(message);
        }

        /// <summary>
        /// Register to add log message handlers
        /// </summary>
        public static System.Action<string> LogHandler;
    }
}
