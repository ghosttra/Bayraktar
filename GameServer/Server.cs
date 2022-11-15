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
    public class GameServer:IDisposable
    {
        public  IPEndPoint ServerEndPoint { get; }

        public IPAddress ServerAddress => ServerEndPoint.Address;
        
        public Action<GameServer> EndGame;
        public GameServer(IPEndPoint endPoint)
        {
            ServerEndPoint = endPoint;
        }
        //public Task SendMessageAsync(MessagePacket message) => Task.Factory.StartNew(() => SendMessage(message));
        //public void SendMessage(MessagePacket message)
        //{
        //    var buffer = message.ToBytes();
        //    new UdpClient().Send(buffer, buffer.Length, ServerEndPoint);
        //}
        
        public void End()
        {
            EndGame?.Invoke(this);
        }
        public void Dispose()
        {
            End();
        }
    }
}
