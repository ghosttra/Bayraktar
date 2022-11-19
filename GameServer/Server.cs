using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
            _timer.Interval = 2000;
            _timer.Elapsed += Timer_Elapsed;
        }
        public Task SendMessageAsync(MessagePacket message) => Task.Factory.StartNew(() => SendMessage(message));
        public void SendMessage(MessagePacket message)
        {
            var buffer = message.ToBytes();
            new UdpClient().Send(buffer, buffer.Length, ServerEndPoint);
        }

        private Timer _timer = new Timer();

        public void StartAutoAttack()
        {
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _attack();
        }
        private Random _random = new Random();
        private int _CritAttackChanse = 10;
        private void _attack()
        {
            var attack = new MessageCommand() { Command = "ATTACK" };
            SendMessage(attack);
            if (_random.Next(_CritAttackChanse) == 0)
                _attack();
        }

        public void Stop()
        {
            _timer.Stop();
        }
        public void End()
        {
            Stop();
            EndGame?.Invoke(this);
        }
        public void Dispose()
        {
            End();
        }
    }
}
