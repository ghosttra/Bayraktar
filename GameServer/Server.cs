using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Message;

namespace GameServer
{
    public class Server
    {
        private readonly IPEndPoint _serverEndPoint;
        public Server(IPEndPoint endPoint)
        {
            _serverEndPoint = endPoint;
        }
        public Task SendMessageAsync(MessagePacket message) => Task.Factory.StartNew(() => SendMessage(message));
        public void SendMessage(MessagePacket message)
        {
            var buffer = message.ToBytes();
            new UdpClient().Send(buffer, buffer.Length, _serverEndPoint);
        }

    }
}
