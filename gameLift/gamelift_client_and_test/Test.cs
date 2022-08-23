using Aws.GameLift.Realtime;
using Aws.GameLift.Realtime.Types;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gamelift_client_and_test
{
    public class Test
    {
        List<GameLiftClient> clients = new List<GameLiftClient>();
        string endpoint = "127.0.0.1";
        int remoteTcpPort = 7654;
        int listeningUdpPort = 9080;
        ConnectionType connectionType = ConnectionType.RT_OVER_WS_UDP_UNSECURED;
        byte[] connectionPayload = new byte[] { };
        string findGameSessionUrl = "https://x2laoqv5cxnfxpjrn7jpn5eidm0uutut.lambda-url.eu-central-1.on.aws"; // Frankfurt
        int firstHour = -1;
        int[] amountOfPlayersByHour = new int[] {
            4800, //00:00
            3300, //01:00
            2300, //02:00
            1800, //03:00
            1300, //04:00
            1000, //05:00
            900,  //06:00
            800,  //07:00
            900,  //08:00
            1100, //09:00
            1500, //10:00
            2400, //11:00
            3400, //12:00
            4300, //13:00
            4200, //14:00
            3700, //15:00
            3600, //16:00
            3700, //17:00
            4250, //18:00
            4700, //19:00
            5400, //20:00
            5900, //21:00
            6500, //22:00
            6100, //23:00
        };

        public bool Execute()
        {
            // Change amount of players every 1h
            // Get current amount of players for feature hour
            if (firstHour.Equals(-1))
            {
                firstHour = GetCurrentHour();
            } else if (firstHour.Equals(GetCurrentHour()))
            {
                // completed
                return true;
            }
            var featureAmountOfPlayersForNextHour = amountOfPlayersByHour[GetCurrentHour()];
            // Calculate how many disconnect and how many connect
            if (clients.Count >= featureAmountOfPlayersForNextHour) {
                var amountOfPlayersToDisconnect = clients.Count - featureAmountOfPlayersForNextHour;
                removeClients(amountOfPlayersToDisconnect);
            } else {
                var amountOfPlayersToConnect = featureAmountOfPlayersForNextHour - clients.Count;
                addClients(amountOfPlayersToConnect);
            }
            return false;
        }

        public void ExecuteMessages()
        {
            if (clients.Count < 0) return;
            // Random Select clients and send event Location
            for (var i = 0; i < clients.Count; i++) {
                clients[i].SendRandomLocation(DeliveryIntent.Fast);
            }
        }

        private void removeClients(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                clients.Last().Disconnect();
                clients.RemoveAt(clients.Count - 1);
            }
        }

        private void addClients(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                PlayerSessionResponse gameSessionResponse = FindGameSession().Result;
                var newGameClient = new GameLiftClient(endpoint, remoteTcpPort, listeningUdpPort, connectionType, gameSessionResponse.playerSessionId, connectionPayload);
                clients.Add(newGameClient);
            }
        }

        public static int GetCurrentHour()
        {
            return DateTime.Now.Hour;
        }

        private async Task<PlayerSessionResponse> FindGameSession()
        {
            HttpClient httpClient = new HttpClient();
            HttpContent httpContent = new ByteArrayContent(new byte[] { });
            PlayerSessionResponse serverReply = new PlayerSessionResponse();

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(findGameSessionUrl, httpContent).ConfigureAwait(false);
            }
            catch (HttpRequestException err)
            {
                Console.WriteLine("hre.Message");
            }

            return serverReply;
        }
    }
}
