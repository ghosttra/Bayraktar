using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using BayraktarGame;
using Message;
using Timer = System.Timers.Timer;

namespace BayraktarClient
{
    public class GameClient
    {
        public IPEndPoint Server { get; }
        protected UdpClient _client;
        public User User { get; }

        private CancellationTokenSource _cts;
        private CancellationToken _token;

        public List<Unit> Units;
        private const short _maxSpeed = 15;
        private const short _minSpeed = 5;
        public int MaxWidth = 2000;

        private int _hp = 1;
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
            HealthPoints = 5;
            Send(new MessageNull());
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
                Send(new MessageCommand() { Command = "GAME_OVER" });
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
        public Action<int> DestroyUnit;

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
                    switch (command.Command)
                    {
                        case "SHOOT":
                            HealthPoints--;
                            break;
                        case "DESTROY":
                            DestroyUnit?.Invoke((int)command.Additional);
                            break;
                        case "ATTACK":
                            SetUnit();
                            break;
                        case "GAME_OVER":
                            GameOver?.Invoke(this);
                            IsRun  = false;
                            break;
                    }
                    break;
            }
        }
        public void Shoot()
        {
            MessageCommand command = new MessageCommand
            {
                Command = "SHOOT"
            };
            Send(command);
        }
        
        protected Random _random = new Random();
        protected List<int> _unitsOnField = new List<int>();
        public void SetUnit(int x)
        {
            MessageUnit messageUnit = new MessageUnit(Units[_random.Next(Units.Count)], x, 0)
            {
                Speed = _random.Next(_minSpeed, _maxSpeed), Id = _generateUnitId()
            };
            Send(messageUnit);
        }
        public void SetUnit()
        {
            SetUnit(_random.Next(MaxWidth));

        }

        private int _generateUnitId()
        {
            var count = _unitsOnField.Count;
            
            var id = count == 0 ? 0 : _unitsOnField[count - 1] + 1;
            _unitsOnField.Add(id);
            return id;
        }
        
        public Action<string> Info;
        public bool Win { get; set; } = false;
        public bool IsRun { get; set; }

        public void Exit()
        {
            Info?.Invoke($"User {User.Login} has leaved");
            End();
        }
        
        
        public void UnitDestroy(int unitTag)
        {
            _unitsOnField.Remove(unitTag);
            Score += _random.Next(50, 100);
            HealthPoints++;
            Send(new MessageCommand() { Command = "DESTROY", Additional = unitTag });
        }
    }
    

}
