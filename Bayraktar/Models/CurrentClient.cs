using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Documents;
using System.Windows.Forms;
using BayraktarClient;
using BayraktarGame;
using dotenv.net;
using Message;
using UserGameClient;

namespace Bayraktar
{
    public class CurrentClient
    {
        public UserClient Client { get; private set; }

        private CurrentClient()
        {
        }

       
        private static CurrentClient _instance;

        public static CurrentClient Instance => _instance ?? (_instance = new CurrentClient());

        public void Send(MessagePacket message)
        {
            Client?.Send(message);
        }
        public MessagePacket Receive()=> Client.Receive();

        public void Init()
        {
            try
            {
                var env = DotEnv.Read();
                var ip = IPAddress.Parse(env["SERVER_IP"].Trim());
                var port = Int32.Parse(env["SERVER_PORT"]);
                Client = new UserClient(ip, port);
            }
            catch
            {
                // ignored
            }
        }
        
        public void GetRating()
        {   
            Client.SendCommand("RATING");
        }

        public GameClient GameConnection;
        public Action<GameClient, GameRole> StartGame;
        public void StartSingleGame()
        {
            Client.SendCommand("SINGLE");
            var game  =Client.Receive();
            if (game is MessageGameData data)
            {
                GameConnection = new GameClient(Client.User, 1000, data.Server);
                StartGame?.Invoke(GameConnection, data.GameRole);
            }
        }

        public void StartMultiGame()
        {
            Client.SendCommand("MULTI");
            var game = Client.Receive();
            if (game is MessageGameData data)
            {
                GameConnection = new GameClient(Client.User, 1000, data.Server);
                StartGame?.Invoke(GameConnection, data.GameRole);
            }
        }
    }
}