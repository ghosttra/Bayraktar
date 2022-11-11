using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Message;

namespace GServer
{
    public class GameServer
    {
        private readonly IPEndPoint _serverEndPoint;
        public GameServer(IPEndPoint endPoint)
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
