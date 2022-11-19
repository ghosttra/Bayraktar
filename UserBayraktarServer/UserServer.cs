using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BayraktarClient;
using BayraktarGame;
using GameEntities;
using Message;
using GServer;

namespace UserBayraktarServer
{
    public class UserServer
    {
        private TcpListener _server;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        public event Action<string> Inform;
        GameContext _context;

        public UserServer(IPAddress address, int port)
        {
            _server = new TcpListener(address, port);
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            Stopped += () => Inform?.Invoke("Server Stopped");

            _context = new GameContext();
        }

        private bool _isRun;
        public Task StartAsync() => Task.Factory.StartNew(Start, _token);
        public async void Start()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            _isRun = true;
            try
            {
                Inform?.Invoke("Starting server");
                _server.Start();
                Inform?.Invoke("Wait for connection");
                while (_isRun)
                {
                    try
                    {
                        await Task.Factory.StartNew(_accept, _token);
                    }
                    catch (Exception e)
                    {
                        Inform?.Invoke(e.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Inform?.Invoke(e.Message);
            }
            finally
            {
                if (_server != null)
                {
                    _stop();
                }
            }
        }
        public event Action Stopped;

        private async void _stop()
        {
            
            if(!_isRun)
                return;

            await Task.Run(_disconnectAllUsers, _token);

            _isRun = false;
            _cts.Cancel();
            _server.Stop();
            Stopped?.Invoke();
        }

        private void _disconnectAllUsers()
        {
            _users.ForEach(u =>
            {
                u.CloseConnection -= CloseConnection;
                u.Disconnect();
            });
            _users.Clear();
        }

        private void _accept()
        {
            var client = _server.AcceptTcpClient();
            Inform?.Invoke("Accept connection");
            Task.Factory.StartNew(() => _connect(client), _token);

        }

        private List<UserConnection> _users = new List<UserConnection>();
        private List<UserConnection> _waitingUsers = new List<UserConnection>();
        private void _connect(TcpClient client)
        {
            var clientConnection = new UserConnection(client);
            var auth = clientConnection.Authorize();
            if (auth == null) clientConnection.Close();
            var user = _authorize(auth);
            var authorize = user != null;
            clientConnection.Send(new MessageBool(authorize));
            if (!authorize)
            {
                Inform?.Invoke($"Authorization failed");
                clientConnection.Close();
                return;
            }
            Inform?.Invoke($"User started connection");
            clientConnection.User = user;
            _users.Add(clientConnection);
            clientConnection.Query += Query;
            clientConnection.InQueueEvent += InQueueEvent;
            clientConnection.CloseConnection += CloseConnection;

            Task.Factory.StartNew(clientConnection.RunAsync, _token);
        }


        private void InQueueEvent(UserConnection user, bool inQueue)
        {
            if (inQueue)
            {
                _waitingUsers.Add(user);
                if (_waitingUsers.Count >= 2)
                {
                    var attack = _waitingUsers[0];
                    var defense = _waitingUsers[1];
                    _waitingUsers.Remove(attack);
                    _waitingUsers.Remove(defense);
                    _startMultiGame(attack, defense);
                }
            }
            else
                _waitingUsers.Remove(user);
        }
        private List<GameServer> _games = new List<GameServer>();

        private MessageGameData _createGameDataMessage(GameRole role, IPEndPoint server)
        {
            var unitData = _context.Units.ToList();
            return new MessageGameData
            {
                Units = unitData, GameRole = role, Server = server
            };
        }

        private void _startMultiGame(UserConnection attack, UserConnection defense)
        {
            var gameServer = _createNewGame();
            attack.Send(_createGameDataMessage(GameRole.Attack, gameServer.ServerEndPoint));
            defense.Send(_createGameDataMessage(GameRole.Defense, gameServer.ServerEndPoint));
        }
        private void _getServerMulti(UserConnection user)
        {
            user.WaitingForMultiplayer();
        }

        private void _getServerSingle(UserConnection user)
        {
            var gameServer = _createNewGame();
            user.Send(_createGameDataMessage(GameRole.Defense, gameServer.ServerEndPoint));
            gameServer.StartAutoAttack();
           // new AutomaticGameClient(gameServer.ServerEndPoint){Units = _context.Units.ToList()};
            //var bot =new AutomaticGameClient(_createGameDataMessage(GameRole.Defense, gameServer.ServerEndPoint));

        }
        private GameServer _createNewGame()
        {
            var ipAddress = _generateIp();
            var endPoint = new IPEndPoint(ipAddress, 1000);
            var game = new GameServer(endPoint);
            _games.Add(game);
            return game;
        }

        private IPAddress _generateIp()
        {
            //235.0. 0.0 through 239.255. 255.255.
            if (_games.Count == 0 || _games[_games.Count - 1].ServerAddress.Equals(IPAddress.Parse("239.255.255.255")))
            {
                return IPAddress.Parse("235.0.0.0");
            }
            return _games[_games.Count - 1].ServerAddress.GetNext();
        }


        private void CloseConnection(UserConnection user)
        {
            _users.Remove(user);
            Inform?.Invoke($"User closed connection");
        }

        private void Query(UserConnection user, MessagePacket message)
        {
            if (message is MessageCommand command)
            {
                switch (command.Command)
                {
                    case "RATING":
                        var rating = _getRating();
                        rating.Description = command.Command;
                            user.Send(rating);
                        break;
                    case "DISCONNECTED":
                        user.Disconnect();
                        break;
                    case "SINGLE":
                        _getServerSingle(user);
                        break;
                    case "MULTI":
                        _getServerMulti(user);
                        break;
                    case "STOP_WAITING":
                        user.StopWaitingFoeMultiplayer();
                        break;
                    case "GAME_OVER":
                        if (command is MessageGameResult gameResult)
                        {
                            _stopServer(gameResult.Server);
                            _saveResult(gameResult);
                        }
                        break;
                }
            }
        }

        private void _stopServer(IPEndPoint gameResultServer)
        {
            var games = _games.Where(g => g.ServerEndPoint.Equals(gameResultServer)).ToList();
            foreach (var game in games)
            {
                game.End();
                _games.Remove(game);
            }
            
        }

        private void _saveResult(MessageGameResult gameResult)
        {
            _context.Statistics.Add(new Statistic
                { Date = DateTime.Now, User = gameResult.User, Score = gameResult.Score });
            _context.SaveChanges();

        }


        #region Commands



        private MessageDataContent _getRating()
        {
            return new MessageDataContent(_context.Statistics.Include("User").ToList());
        }

        private MessageDataContent _getUnits()
        {
            return new MessageDataContent(_context.Units.ToList());
        }
        private User _authorize(MessageAuthorize auth)
        {
            switch (auth.Mode)
            {
                case AuthorizeMode.Registration:
                    if (_context.Users.Any(u => u.Login.Equals(auth.Login)))
                        return null;
                    var registration = new User()
                    {
                        Login = auth.Login,
                        PassWord = auth.Password
                    };
                    _context.Users.Add(registration);
                    _context.SaveChangesAsync(_token);
                    Inform?.Invoke($"New user was registered");
                    return registration;
                case AuthorizeMode.Login:
                    var user = _context.Users.FirstOrDefault(u => u.Login.Equals(auth.Login));
                    if (user == null) return null;
                    if (!user.PassWord.Equals(auth.Password)) return null;
                    return _users.All(u => u.User?.Equals(user) == false) ? user : null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion


        public void Stop()
        {
            _stop();
        }
    }

    internal static class Extension
    {
        public static IPAddress GetNext(this IPAddress ipAddress, uint increment = 1)
        {
            byte[] addressBytes = ipAddress.GetAddressBytes().Reverse().ToArray();
            uint ipAsUint = BitConverter.ToUInt32(addressBytes, 0);
            var nextAddress = BitConverter.GetBytes(ipAsUint + increment);
            return IPAddress.Parse(String.Join(".", nextAddress.Reverse()));
        }
    }
}
