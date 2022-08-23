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
using System.IO;
using Aws.GameLift.Realtime.Command;
using Com.Gamelift.Rt.Proto;

namespace Aws.GameLift.Realtime.Network
{
    /// <summary>
    /// Base class for all connections (websocket, udp) used in GameLift Realtime client.
    /// </summary>
    public abstract class BaseConnection : NetworkEvents, Connection, IDisposable
    {
        protected ConnectionStats stats = new ConnectionStats();

        public BaseConnection() { }

        /// <summary>
        /// Initializes a connection with a server
        /// </summary>
        /// <param name="hostName">hostName of server</param>
        /// <param name="port">port of server</param>
        public abstract void Initialize(string hostName, int port);

        /// <summary>
        /// Open the initialized connection with the server.
        /// This should be called after Initialize(string, string)
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// Called before packets are sent to the server.
        /// </summary>
        /// <param name="packet">Packet to be sent</param>
        protected virtual void BeforeSend(Packet packet) { }

        /// <summary>
        /// Decides whether if the packet can be sent to the server.
        /// </summary>
        /// <returns><c>true</c>, if packet can be sent, <c>false</c> otherwise.</returns>
        /// <param name="packet">Packet to be sent</param>
        protected virtual bool CanSend(Packet packet) { return true; }

        /// <summary>
        /// Called after packets are sent to the server.
        /// </summary>
        /// <param name="packet">Packet sent</param>
        protected virtual void AfterSend(Packet packet) { }

        /// <summary>
        /// Sends data to server using the established connnection. 
        /// Note: The data was transformed to byte array using Protobuf.
        /// </summary>
        /// <param name="data">Raw byte array of packet</param>
        /// <param name="len">Length of data</param>
        protected abstract void SendData(byte[] data, int len);

        /// <summary>
        /// Called before packets are received.
        /// </summary>
        /// <param name="packet">Packet to be received</param>
        protected virtual void BeforeReceive(Packet packet) { }

        /// <summary>
        /// Decides whether if packets can be received from the server.
        /// </summary>
        /// <returns><c>true</c>, if client can receive the packet, <c>false</c> otherwise.</returns>
        /// <param name="packet">Packet.</param>
        protected virtual bool CanReceive(Packet packet) { return true; }

        /// <summary>
        /// Called after packets are received.
        /// NOTE: for business logics, the packet should be handled by OnMessageReceived(),
        ///       this method is used for clean up if necessary.
        /// </summary>
        /// <param name="packet">Packet received</param>
        protected virtual void AfterReceive(Packet packet) { }

        /// <summary>
        /// Close the established connection.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Dispose the connection if needed.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> dispose the connection.</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Send a packet to server.
        /// Note: unless you want to serialize the data using methods other than protobuf,
        ///       override SendData(byte[], int) instead.
        /// </summary>
        /// <returns>The send.</returns>
        /// <param name="packet">Packet.</param>
        public virtual int Send(Packet packet)
        {
            try
            {
                BeforeSend(packet);

                if (!CanSend(packet))
                {
                    return -1;
                }

                int messageSize = packet.CalculateSize();

                using (MemoryStream ms = new MemoryStream())
                {
                    var output = new Google.Protobuf.CodedOutputStream(ms, true);
                    output.WriteMessage(packet);
                    int ret = (int)output.Position;
                    output.Flush();

                    byte[] data = ms.ToArray();
                    ClientLogger.Info("Sending packet with size {0}, opcode: {1}, content: {2}", data.Length, packet.OpCode, packet.ToString());
                    SendData(data, data.Length);
                    stats.RecordMessageSent();

                    AfterSend(packet);

                    return ret;
                }
            }
            catch (Exception e)
            {
                ClientLogger.Error("Exception occurred sending data. Exception: {0}", e.Message);
            }
            return -1;
        }

        /// <summary>
        /// Called when a packet is received.
        /// Note: unless you want to serialize the data using methods other than protobuf,
        ///       override OnMessageReceived() instead to handle the packet that was 
        ///       serialized by protobuf.
        /// </summary>
        /// <param name="packet">Packet.</param>
        public virtual void OnPacketReceived(Packet packet)
        {
            stats.RecordMessageReceived();

            BeforeReceive(packet);

            if (!CanReceive(packet))
            {
                ClientLogger.Warn("Packet was not allowed to be received.");
                return;
            }

            RTMessage response = RTMessage.FromPacket(packet);

            OnMessageReceived(response);

            AfterReceive(packet);
        }

        /// <summary>
        /// Gets the connection stats.
        /// </summary>
        /// <returns>connection stats</returns>
        public ConnectionStats GetStats()
        {
            return stats.GetCopy();
        }

        /// <summary>
        /// Resets the connection stats.
        /// </summary>
        public void ResetStats()
        {
            stats.Reset();
        }

        /// <summary>
        /// Terminate the connection.
        /// </summary>
        public void Terminate()
        {
            Close();
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Aws.GameLift.Realtime.Network.BaseConnection"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:Aws.GameLift.Realtime.Network.BaseConnection"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:Aws.GameLift.Realtime.Network.BaseConnection"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Aws.GameLift.Realtime.Network.BaseConnection"/> so the garbage collector can reclaim the memory
        /// that the <see cref="T:Aws.GameLift.Realtime.Network.BaseConnection"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
