using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BayraktarGame;
using Message;

namespace BayraktarClient
{
    public class GameClient
    {
        private IPEndPoint _server;
        private UdpClient _client;
        public User User { get; }
        private CancellationTokenSource _cts;
        private CancellationToken _token;

        //public short MaxSpeed { get; set; } = 5;
        //public short MinSpeed { get; set; } = 15;

        private int _hp = 5;
        public int HealthPoints
        {
            get
            => _hp;
            set
            {
                _hp = value;
                HealthChanged?.Invoke(HealthPoints);

                if (value<= 0)
                    GameOver?.Invoke(false); //проигрыш
            }
        }

        public Action<int> HealthChanged;
        public Action<bool> GameOver;
        public GameClient(User user, int localPort, IPEndPoint server)
        {
            User = user;
            _server = server;
            _client = new UdpClient(localPort);
            _client.JoinMulticastGroup(_server.Address, 50);
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            Task.Factory.StartNew(_start, _token);
        }

        private void _start()
        {
            while (true)
            {
                _receive();

            }
        }
        public void Send(MessagePacket message)
        {
            var buffer = message.ToBytes();
            new UdpClient().Send(buffer, buffer.Length, _server);
        }

        private void _receive()
        {
            IPEndPoint endPoint = null;
            var buffer = _client.Receive(ref endPoint);
            _handle(buffer);
        }

        private void _handle(byte[] buffer)
        {

            var message = MessagePacket.FromBytes(buffer);
            //пример обработки
            switch (message)
            {
                case MessageCommand command:
                    if (command.Command == "SHOOT")
                        HealthPoints--;
                    break;
            }
        }
        public void Shoot(double x, double y)
        {
            //Пример функции
            MessageCommand command = new MessageCommand
            {
                Command = "SHOOT"
            };
            Send(command);
        }

        public void GetAllUnits()
        {

        }

        public void SetUnit(Unit unit, int x, int y)
        {

        }
    }

    public class AutomaticGameClient: GameClient
    {
        public AutomaticGameClient(IPEndPoint server) : base(new ComputerUser(), localPort, server)
        {
        }
    }
}
