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
using Aws.GameLift.Realtime.Types;
using Aws.GameLift.Realtime.Event;
using Aws.GameLift.Realtime.Command;
using Aws.GameLift.Realtime.Network;

namespace Aws.GameLift.Realtime
{
    /// <summary>
    /// Base client to connect to a GameLift Realtime server.
    /// </summary>
    public class Client : ClientEvents
    {
        /// <summary>Version number of Realtime Client SDK.</summary>
        public const string clientVersion = "1.2.0";

        /// <summary>Maximum message size for reliable messages.</summary>
        /// <remarks>This limit applies to the payload field of messages.</remarks>
        public const int maxReliableMessageBytes = 4096;

        /// <summary>Maximum message size for fast messages.</summary>
        /// <remarks>This limit applies to the payload field of messages.  Messages larger than
        ///          this limit, but smaller than maxReliableMessageBytes will fallback to the
        ///          reliable connection instead.</remarks>
        ///
        // NOTE: This value is to stay under the ethernet MTU of 1500 even after adding message
        //       header fields and the overhead of (future) DTLS.
        public const int maxFastMessageBytes = 1200;

        // Parameters controlling retrying during UDP handshake
        private const int InitialUdpRetryMillis = 100;
        private const int MaxUdpRetryMillis = 5 * 1000;

        /// <summary>Developer client version. Sent during connect.</summary>
        /// <remarks>This is only sent when you connect, can be used by sever scripts to separate incompatible clients.</remarks>
        public static string GameVersion { get; set; }

        /// <summary>Holds information about the connected session for the cluent.</summary>
        public ClientSession Session { get; private set; }

        /// <summary>
        /// False until client connects to GameLift Realtime. True while connected to any server.
        /// </summary>
        public bool Connected
        {
            get
            {
                return (reliableConnection != null &&
                    (connectionStatus.Status == ConnectionStatus.CONNECTED ||
                     connectionStatus.Status == ConnectionStatus.CONNECTED_SEND_FAST ||
                     connectionStatus.Status == ConnectionStatus.CONNECTED_SEND_AND_RECEIVE_FAST));
            }
        }

        /// <summary>
        /// A refined version of connected which is true only if the connection to the server is ready to accept operations like join, leave, etc.
        /// </summary>
        public bool ConnectedAndReady
        {
            get
            {
                return Connected && Session.LoggedIn;
            }
        }

        private ClientConfiguration clientConfiguration;
        private ConnectionFactoryOptions connectionFactoryOptions;
        private ConnectionStatusHolder connectionStatus;
        private Connection reliableConnection; // currently websockets
        private Connection fastConnection;     // currently udp
        private int listenUdpPort = Constants.INVALID_PORT;
        private string remoteEndpoint;

        [Obsolete("This client is unsecured by default, prefer to use secure client instead. " +
                  "See TLS Support on GameLift Documentation")]
        public Client()
            : this(ClientConfiguration.Default())
        {
        }

        public Client(ClientConfiguration configuration)
            : this(configuration, null, new ConnectionStatusHolder())
        {
        }

        [Obsolete("This client is unsecured by default, prefer to use secure client instead. " +
                  "See TLS Support on GameLift Documentation")]
        public Client(Connection reliableConnection, ConnectionStatusHolder connectionStatus)
            : this(ClientConfiguration.Default(), reliableConnection, new ConnectionStatusHolder())
        {
        }

        public Client(ClientConfiguration clientConfiguration,
                      Connection reliableConnection,
                      ConnectionStatusHolder connectionStatus)
            : this(clientConfiguration,
                   reliableConnection,
                   new ClientSession(string.Empty, -1, null),
                   connectionStatus)
        {
        }

        public Client(ClientConfiguration clientConfiguration,
                      Connection reliableConnection,
                      ClientSession clientSession,
                      ConnectionStatusHolder connectionStatus)
        {
            this.clientConfiguration = clientConfiguration;
            this.reliableConnection = reliableConnection;
            this.connectionStatus = connectionStatus;
            Session = clientSession;
        }

        /// <summary>
        /// Connect to a Realtime server
        /// </summary>
        /// <param name="endpoint">The endpoint to connect to, for example the IpAddress returned for a game session</param>
        /// <param name="remoteTcpPort">The Realtime server's TCP port, for example the port of the game session</param>
        /// <param name="listenPort">The local client-side UDP listen port</param>
        /// <param name="token">The connection token to include</param>
        /// <returns></returns>
        public ConnectionStatus Connect(string endpoint, int remoteTcpPort,
                                        int listenPort, ConnectionToken token)
        {
            this.remoteEndpoint = endpoint;
            this.listenUdpPort = listenPort;
            this.connectionFactoryOptions = new ConnectionFactoryOptions()
            {
                UdpListenPort = listenPort,
                HostName = endpoint,
                ClientConfiguration = clientConfiguration
            };

            if (reliableConnection == null)
            {
                BaseConnection wsConnection = clientConfiguration.ReliableConnectionFactory.Create(connectionFactoryOptions);
                connectionStatus.Status = ConnectionStatus.READY;

                wsConnection.ConnectionOpen += OnConnectionOpen;
                wsConnection.ConnectionClose += OnConnectionClose;
                wsConnection.ConnectionError += OnConnectionError;
                wsConnection.MessageReceived += OnMessageReceived;

                Session = new ClientSession(endpoint, remoteTcpPort, token);
                reliableConnection = wsConnection;
                reliableConnection.Initialize(endpoint, remoteTcpPort);
                connectionStatus.Status = ConnectionStatus.CONNECTING;
                reliableConnection.Open();
            }
            return connectionStatus.Status;
        }

        public void Disconnect()
        {
            connectionStatus.Status = ConnectionStatus.DISCONNECTED_CLIENT_CALL;
            if (reliableConnection != null)
            {
                reliableConnection.Close();
                reliableConnection = null;
            }
            if (fastConnection != null)
            {
                fastConnection.Close();
                fastConnection = null;
            }
        }

        public void JoinGroup(int targetGroup)
        {
            Send(new JoinGroup(Session.ConnectedPeerId, targetGroup));

            Session.JoinGroup(targetGroup);
        }

        public void LeaveGroup(int targetGroup)
        {
            Send(new LeaveGroup(Session.ConnectedPeerId, targetGroup));

            Session.LeaveGroup(targetGroup);
        }

        public void RequestGroupMembership(int targetGroup)
        {
            Send(new RequestGroupMembership(Session.ConnectedPeerId, targetGroup));
        }

        /// <summary>
        /// Create a new Realtime Message using the passed OpCode and clients connected identity
        /// </summary>
        /// <param name="opCode">The opCode to set for the message</param>
        /// <returns>A Realtime message object</returns>
        public RTMessage NewMessage(int opCode)
        {
            return new RTMessage(opCode, Session.ConnectedPeerId);
        }

        /// <summary>
        /// Send a general RealtimeMesage via the server</summary>
        /// <remarks>
        /// If targetPlayer == Constants.PLAYER_ID_SERVER then message is sent to the server script's onMessage handler 
        /// If targetPlayer == <player id> then message is sent to the server script's onSendToPlayer handler.
        /// If targerGroup == <group id> hen message is sent to the server script's onSendToGroup handler.
        /// </remarks>
        ///
        /// <param name="message">The message to send</param>
        public void SendMessage(RTMessage message)
        {
            Send(message);
        }

        /// <summary>
        /// Sends a Realtime ClientEvent to the server (client to server message)
        /// </summary>
        /// <param name="opCode">The developer defined op code for the event</param>
        /// <param name="data">An optional payload to set on the event</param>
        public void SendEvent(int opCode, byte[] data = null)
        {
            Send(new ClientEvent(opCode, Session.ConnectedPeerId, data));
        }

        // Check if UDP communication is set up for both client and server
        public bool CanSendFast()
        {
            return connectionStatus.Status == ConnectionStatus.CONNECTED_SEND_FAST ||
                connectionStatus.Status == ConnectionStatus.CONNECTED_SEND_AND_RECEIVE_FAST;
        }

        // Get UDP statistics for the client
        public ConnectionStats GetFastConnectionStats()
        {
            return fastConnection.GetStats();
        }

        // Get TCP statistics for the client
        public ConnectionStats GetReliableConnectionStats()
        {
            return reliableConnection.GetStats();
        }

        // Reset the client's statistics sets
        public void ResetStats()
        {
            if (fastConnection != null)
            {
                fastConnection.ResetStats();
            }
            if (reliableConnection != null)
            {
                reliableConnection.ResetStats();
            }
        }

        private void OnConnectionOpen(object sender, EventArgs args)
        {
            connectionStatus.Status = ConnectionStatus.CONNECTED;

            // Send login command
            LoginCommand loginCommand =
                new LoginCommand(Session.Token.PlayerSessionId, Session.Token.Payload, Session.ConnectedPeerId);

            Send(loginCommand);

            // Signal listeners
            OnOpen();
        }

        private void OnConnectionClose(object sender, EventArgs args)
        {
            Session.LoggedIn = false;
            // Closing for a reason other than client request
            if (connectionStatus.Status != ConnectionStatus.DISCONNECTED_CLIENT_CALL)
            {
                connectionStatus.Status = ConnectionStatus.DISCONNECTED;
            }

            // Signal listeners
            OnClose();
        }

        private void OnConnectionError(object sender, ErrorEventArgs args)
        {
            // Signal listeners
            OnError(args.Exception);
        }

        private void OnMessageReceived(object sender, MessageEventArgs args)
        {
            RTMessage result = args.Result;

            switch (result.OpCode)
            {
                case Constants.LOGIN_RESPONSE_OP_CODE:
                    HandleLoginResponse((LoginResult)result);
                    break;

                case Constants.GROUP_MEMBERSHIP_UPDATE_OP_CODE:
                    HandleGroupMembershipUpdate((GroupMembershipUpdate)result);
                    break;

                case Constants.UDP_CONNECT_SERVER_ACK_OP_CODE:
                    HandleUDPServerAck();
                    break;

                case Constants.VERIFY_IDENTITY_RESPONSE_OP_CODE:
                    HandleVerifyIdentityResponse((VerifyIdentityResult) result);
                    break;

                default:
                    // Signal to developer
                    OnDataReceived(result.Sender, result.OpCode, result.Payload);
                    break;
            }
        }

        private void HandleVerifyIdentityResponse(VerifyIdentityResult result)
        {
            if (result.Success)
            {
                if (connectionStatus.Status == ConnectionStatus.CONNECTED)
                {
                    connectionStatus.Status = ConnectionStatus.CONNECTED_SEND_FAST;
                }
            }
        }

        private void HandleLoginResponse(LoginResult result)
        {
            LoginResult loginResult = result;
            Session.ConnectedPeerId = result.TargetPlayer;
            Session.LoggedIn = loginResult.Success;

            if (loginResult.Success)
            {
                // Once logged in over websocket we need to setup our UDP communication channel
                if (fastConnection == null)
                {
                    int remoteUdpPort = result.UdpPort;
                    connectionFactoryOptions.CaCert = loginResult.CaCert;
                    BaseConnection udpConnection = clientConfiguration.FastConnectionFactory.Create(connectionFactoryOptions);
                    fastConnection = udpConnection;
                    udpConnection.Initialize(remoteEndpoint, remoteUdpPort);
                    udpConnection.MessageReceived += OnMessageReceived;
                    udpConnection.Open();
                }

                if (this.clientConfiguration.ConnectionType == ConnectionType.RT_OVER_WSS_DTLS_TLS12)
                {
                    // DTLS handshake occurred with opening connection. Verify identity now.
                    var verifyIdentityCommand =
                        new VerifyIdentityCommand(Session.ConnectedPeerId, loginResult.ConnectToken, Session.Token.Payload, Session.ConnectedPeerId);

                    fastConnection.Send(verifyIdentityCommand.ToPacket());
                }
                else
                {
                    // Not DTLS, so do a regular UDP handshake.
                    InitiateUDPHandshake(0);
                }
            }
        }

        private void HandleGroupMembershipUpdate(GroupMembershipUpdate groupMembershipUpdate)
        {
            GroupMembershipEventArgs groupMembershipEventArgs = 
                new GroupMembershipEventArgs(groupMembershipUpdate.Sender, 
                    groupMembershipUpdate.GroupId, groupMembershipUpdate.PlayerIds);

            Session.UpdateGroupMembership(groupMembershipUpdate.GroupId,
                                          groupMembershipUpdate.PlayerIds);

            OnGroupMembershipUpdated(groupMembershipEventArgs);
        }

        private void HandleUDPServerAck()
        {
            if (connectionStatus.Status == ConnectionStatus.CONNECTED)
            {
                connectionStatus.Status = ConnectionStatus.CONNECTED_SEND_FAST;
                UDPClientAckMessage connectMsg = new UDPClientAckMessage(Session.ConnectedPeerId);
                fastConnection.Send(connectMsg.ToPacket());
            }
        }

        private void InitiateUDPHandshake(int iteration)
        {
            if (connectionStatus.Status == ConnectionStatus.CONNECTED)
            {
                // The initial handshake message must be sent over udp to prove to the server that
                // we are able to send on that channel.
                UDPConnectMessage connectMsg = new UDPConnectMessage(Session.ConnectedPeerId);
                fastConnection.Send(connectMsg.ToPacket());

                // retry in a bit since the message might be lost
                int retryMillis =
                    (int)Math.Min(Math.Pow(2, iteration) * InitialUdpRetryMillis, MaxUdpRetryMillis);
                Task.Delay(retryMillis).ContinueWith((antecedent) => InitiateUDPHandshake(iteration + 1));
            }
        }

        internal int Send(RTMessage message)
        {
            int payloadSize = message.Payload != null ? message.Payload.Length : 0;
            if (payloadSize > maxReliableMessageBytes)
            {
                throw new Exception(
                    string.Format("Message payload exceeded maximum size of {0} bytes", maxReliableMessageBytes));
            }

            if (Connected)
            {
                Com.Gamelift.Rt.Proto.Packet packet = message.ToPacket();
                packet.Sender = Session.ConnectedPeerId;

                bool shouldUseReliable = true;
                if (message.DeliveryIntent != DeliveryIntent.Reliable)
                {
                    if (connectionStatus.Status != ConnectionStatus.CONNECTED_SEND_FAST &&
                        connectionStatus.Status != ConnectionStatus.CONNECTED_SEND_AND_RECEIVE_FAST)
                    {
                        ClientLogger.Info("Client requested fast connection, but not currently setup -- "
                            + "falling back to reliable");
                    }
                    else if (payloadSize > maxFastMessageBytes)
                    {
                        ClientLogger.Info("Client requested fast connection, but payload size exceeds "
                            + "limit of {0} bytes -- falling back to reliable", maxFastMessageBytes);
                    }
                    else
                    {
                        shouldUseReliable = false;
                    }
                }

                if (shouldUseReliable)
                {
                    packet.Reliable = true;
                    return reliableConnection.Send(packet);
                }
                else
                {
                    packet.Reliable = false;
                    return fastConnection.Send(packet);
                }
            }
            else
            {
                ClientLogger.Error("Not connected. Unable to send request");
                return Constants.ZERO_MESSAGE_BYTES;
            }
        }
    }

    // NOTE: The existance of this class is to allow for easy mocking of the status by unit
    //       tests.  Currently this is super specific (perhaps overly so), but we might
    //       consider moving other connection-related items into this class in addition.
    public class ConnectionStatusHolder
    {
        public virtual ConnectionStatus Status { get; set; }
    }
}
