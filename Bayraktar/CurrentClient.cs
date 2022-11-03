using System;
using System.Net;
using dotenv.net;
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