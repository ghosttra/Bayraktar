using System;
using System.Net;
using System.Windows.Forms;
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

        public void SendCommand(string command)
        {
            var message = new MessageCommand
            {
                Command = command
            };
            Send(message);
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
    }
}