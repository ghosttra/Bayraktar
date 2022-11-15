﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
        public Action<bool> WaitForGame;
        public async void StartSingleGame()
        {
            Client.SendCommand("SINGLE");
            await Task.Run(_waitForGameServer);
        }

        public async  void StartMultiGame()
        {
            Client.SendCommand("MULTI");
            //MessageConnectionGame connection  = new MessageConnectionGame{GameMode = GameMode.Multiplayer, LocalPort = Client.}
            await Task.Run(_waitForGameServer);
        }

        private void _waitForGameServer()
        {

            WaitForGame?.Invoke(true);
            var game = Client.Receive();
            if (game is MessageGameData data)
            {
                GameConnection = new GameClient(Client.User, 1000, data.Server);
                WaitForGame?.Invoke(false);
                StartGame?.Invoke(GameConnection, data.GameRole);
            }
        }
            
    }
}