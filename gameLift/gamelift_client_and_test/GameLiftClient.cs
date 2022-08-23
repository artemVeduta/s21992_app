using System.Text;
using Aws.GameLift.Realtime;
using Aws.GameLift.Realtime.Event;
using Aws.GameLift.Realtime.Types;
using DataReceivedEventArgs = Aws.GameLift.Realtime.Event.DataReceivedEventArgs;
using Constants = Aws.GameLift.Realtime.Constants;
using Newtonsoft.Json;
using System;

namespace gamelift_client_and_test
{
    class GameLiftClient
    {
        public Aws.GameLift.Realtime.Client Client { get; private set; }

        // OP Code for Message LOCATION
        private const int OP_CODE_LOCATION_MESSAGE = 111;

        // Initialize client
        public GameLiftClient(string endpoint, int remoteTcpPort, int listeningUdpPort, ConnectionType connectionType,
                     string playerSessionId, byte[] connectionPayload)
        {
            ClientConfiguration clientConfiguration = new ClientConfiguration()
            {
                ConnectionType = connectionType
            };

            Client = new Client(clientConfiguration);

            Client.ConnectionOpen += new EventHandler(OnOpenEvent);
            Client.ConnectionClose += new EventHandler(OnCloseEvent);
            Client.GroupMembershipUpdated += new EventHandler<GroupMembershipEventArgs>(OnGroupMembershipUpdate);
            Client.DataReceived += new EventHandler<DataReceivedEventArgs>(OnDataReceived);

            ConnectionToken connectionToken = new ConnectionToken(playerSessionId, connectionPayload);

            Client.Connect(endpoint, remoteTcpPort, listeningUdpPort, connectionToken);
        }

        public void Disconnect()
        {
            if (Client.Connected)
            {
                Client.Disconnect();
            }
        }

        public bool IsConnected()
        {
            return Client.Connected;
        }

        public void SendRandomLocation(DeliveryIntent intent)
        {
            var random = new Random();
            var payload = new LocationMessagePayload(random.Next(0, 100), random.Next(0, 100));
            Client.SendMessage(Client.NewMessage(OP_CODE_LOCATION_MESSAGE)
                .WithDeliveryIntent(intent)
                .WithTargetPlayer(Constants.PLAYER_ID_SERVER)
                .WithPayload(StringToBytes(ClassToString(payload))));
        }

        public void OnOpenEvent(object sender, EventArgs e)
        {
        }

        public void OnCloseEvent(object sender, EventArgs e)
        {
        }

        public void OnGroupMembershipUpdate(object sender, GroupMembershipEventArgs e)
        {
        }

        public virtual void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            switch (e.OpCode)
            {
                // handle messages from server
                default:
                    break;
            }
        }

        public static byte[] StringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string BytesToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ClassToString(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
