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
        public IPEndPoint Server { get; }
        private UdpClient _client;
        public User User { get; }

        private CancellationTokenSource _cts;
        private CancellationToken _token;

        public List<Unit> Units;
        private const short _maxSpeed = 15;
        private const short _minSpeed = 5;
        public int MaxWidth = 300;

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
                    End();
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
            Server = server;
            _client = new UdpClient(localPort);
            _client.JoinMulticastGroup(Server.Address, 50);
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
                _client.Close();
                _client = null;
            }
        }

        public void End()
        {
            if (IsRun)
                IsRun = false;
        }
        private void _start()
        {
            IsRun= true;
            while (IsRun)
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
            new UdpClient().Send(buffer, buffer.Length, Server);
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

        protected Random _random = new Random();
        public void SetUnit(int x)
        {
            MessageUnit messageUnit = new MessageUnit(Units[_random.Next(Units.Count)], x, 0)
            {
                Speed = _random.Next(_minSpeed, _maxSpeed)
            };
            Send(messageUnit);
        }

        public void GetAllUnits()
        {

        }

        public Action<string> Info;
        public bool Win { get; set; } = false;
        private bool _isRun;
        public bool IsRun
        {
            get => _isRun;
            set
            {
                _isRun = value;
                if (!value)
                    GameOver?.Invoke(this);
            }
        }

        public void Exit()
        {
            Info?.Invoke($"User {User.Login} has leaved");
            End();
        }

    }

    public class AutomaticGameClient : GameClient
    {
        public AutomaticGameClient(IPEndPoint server) : base(new User{Login = "BOT"}, 1000, server)
        {
        }

        private bool _isRun;
        private CancellationToken _token;
        private CancellationTokenSource _cts;
        public void Start()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            _isRun = true;
            while (_isRun)
            {
                _attack();
            }
        }

        private void _attack()
        {
            SetUnit(_random.Next());
        }

        public void Stop()
        {
            _isRun = false;
            _cts.Cancel();
        }
        public Task StartAsync()=> Task.Run(Start, _token);
    }
}
