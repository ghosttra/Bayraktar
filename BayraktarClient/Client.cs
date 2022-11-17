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

        public List<Unit> Units;
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

                if (value <= 0)
                    GameOver?.Invoke(this);
            }
        }

        private int _score = 0;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreChanged?.Invoke(value);
            }
        }


        public Action<int> HealthChanged;
        public Action<int> ScoreChanged;
        public Action<GameClient> GameOver;
        public GameClient(User user, int localPort, IPEndPoint server)
        {
            User = user;
            _server = server;
            _client = new UdpClient(localPort);
            _client.JoinMulticastGroup(_server.Address, 50);
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            GameOver += _gameOver;
            Task.Factory.StartNew(_start, _token);
        }

        private void _gameOver(GameClient client)
        {
            try
            {

                _cts?.Cancel();
                _cts?.Dispose();
            }
            finally
            {
                End();

            }
        }

        public void End()
        {
            _client.Close();
            _client = null;

        }
        private void _start()
        {
            while (true)
            {
                try
                {
                    _receive();
                }
                catch (Exception)
                {
                    //ignored
                }
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
            var buffer = _client?.Receive(ref endPoint);
            _handle(buffer);
        }

        public Action<MessageUnit> SetUnitAction;

        private void _handle(byte[] buffer)
        {
            if (buffer == null)
                return;
            var message = MessagePacket.FromBytes(buffer);

            //Пример обработки
            switch (message)
            {

                case MessageUnit unitCommand:
                    SetUnitAction?.Invoke(unitCommand);
                    break;
                case MessageCommand command:
                    if (command.Command.Equals("SHOOT"))
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
        public void SetUnit(Unit unit, int x, int y)
        {
            MessageUnit command = new MessageUnit(unit, x, y);
            Send(command);
        }



        public void GetAllUnits()
        {

        }

        public Action<string> Info;
        public bool Win { get; set; } = false;

        public void Exit()
        {
            Info?.Invoke($"User {User.Login} has leaved");
            GameOver?.Invoke(this);
        }
    }

    //public class AutomaticGameClient: GameClient
    //{
    //    public AutomaticGameClient(IPEndPoint server) : base(new ComputerUser(), 1000, server)
    //    {
    //    }
    //}
}
