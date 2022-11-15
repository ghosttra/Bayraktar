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

        public Action<GameServer> StartGame;
        public Action<GameServer> EndGame;
        public GameServer(IPEndPoint endPoint)
        {
            ServerEndPoint = endPoint;
            Task.Factory.StartNew(_start);
        }
        public Task SendMessageAsync(MessagePacket message) => Task.Factory.StartNew(() => SendMessage(message));
        public void SendMessage(MessagePacket message)
        {
            var buffer = message.ToBytes();
            new UdpClient().Send(buffer, buffer.Length, ServerEndPoint);
        }
        private void _start()
        {
            while (true)
            {
                _receive();

            }
        }
        private void _receive()
        {
            IPEndPoint endPoint = null;
            var buffer = new UdpClient().Receive(ref endPoint);
            _handle(buffer);
            
        }

        private void _handle(byte[] buffer)
        {
            var message = MessagePacket.FromBytes(buffer);
            if (message is MessageCommand command)
            {
                HealthPoints--;
            }
        }
        private int _hp = 5;
        public int HealthPoints
        {
            get
                => _hp;
            set
            {
                _hp = value;
                HealthChanged?.Invoke(HealthPoints);
            }
        }

        public Action<int> HealthChanged;

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
