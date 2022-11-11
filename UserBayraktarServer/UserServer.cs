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
        public Task StartAsync() => Task.Factory.StartNew(Start, _token);
        public async void Start()
        {
            try
            {
                Inform?.Invoke("Starting server");
                _server.Start();
                Inform?.Invoke("Wait for connection");
                while (true)
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

        private void _stop()
        {
            _server.Stop();
            Stopped?.Invoke();
        }

        private void _accept()
        {
            var client = _server.AcceptTcpClient();
            Inform?.Invoke("Accept connection");
            Task.Factory.StartNew(() => _verify(client), _token);

        }

        private List<UserConnection> _users = new List<UserConnection>();
        private List<UserConnection> _waitingUsers = new List<UserConnection>();
        private void _verify(TcpClient client)
        {
            var clientConnection = new UserConnection(client);
            var auth = clientConnection.Authorize();
            if (auth == null) clientConnection.Close();
            var result = _authorize(auth);
            clientConnection.Send(new MessageBool(result));
            if (!result)
            {
                Inform?.Invoke($"Authorization failed");
                clientConnection.Close();
            }
            Inform?.Invoke($"User started connection");
            clientConnection.Query += Query;
            clientConnection.InQueueEvent+= InQueueEvent;
            clientConnection.CloseConnection+=CloseConnection;
            clientConnection.RunAsync();
            _users.Add(clientConnection);
        }

        private void InQueueEvent(UserConnection user, bool inQueue)
        {
            if(inQueue)
            {
                _waitingUsers.Add(user);
                if (_waitingUsers.Count >= 2)
                {
                    var attack =_waitingUsers[0];
                    var defense =_waitingUsers[1];
                    _waitingUsers.Remove(attack);
                    _waitingUsers.Remove(defense);
                    _startMultiGame(attack, defense);
                }
            }
            else
                _waitingUsers.Remove(user);
        }
        private List<IPAddress> _games = new List<IPAddress>();
        private void _startMultiGame(UserConnection attack, UserConnection defense)
        {
            var ip = _createNewGame();
            var server = new GameServer(ip);
        }

        private IPEndPoint _createNewGame()
        {

            var ip = new IPEndPoint(_generateIp(),1000);
            return ip;
        }

        private IPAddress _generateIp()
        {
            //224.0. 0.0 through 239.255. 255.255.
            //if (_games.Count == 0 || _games[_games.Count - 1].Equals(IPAddress.Parse("239.255.255.255")))
            //{
            //    return IPAddress.Parse("224.0.0.0");
            //}

            //var ip = _games[_games.Count - 1];
            var ip = IPAddress.Parse("224.255.255.255");

       
            return ip;
        }

        private void _startSingleGame(UserConnection defense)
        {
            var ip = _createNewGame();

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
                        _getRating(user);
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
                }
            }
        }


        #region Commands

        private void _getServerMulti(UserConnection user)
        {
            user.WaitingForMultiplayer();
        }

        private void _getServerSingle(UserConnection user)
        {
            _startSingleGame(user);
        }


        private void _getRating(UserConnection user)
        {
            var rating = new MessageDataContent();
            try
            {
                var stats = _context.Statistics.Include("User").ToList();
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter form = new BinaryFormatter();
                    form.Serialize(stream, stats);
                    rating.Content = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                user.Send(new MessageNull());
            }
            user.Send(rating);
        }

        #endregion

        private bool _authorize(MessageAuthorize auth)
        {
            switch (auth.Mode)
            {
                case AuthorizeMode.Registration:
                    var us = _context.Users.ToList();
                    if (_context.Users.Any(u => u.Login.Equals(auth.Login)))
                        return false;
                    _context.Users.Add(new User()
                    {
                        Login = auth.Login,
                        PassWord = auth.Password
                    });
                    _context.SaveChangesAsync(_token);
                    Inform?.Invoke($"New user was registered");
                    return true;
                case AuthorizeMode.Login:
                    var user = _context.Users.FirstOrDefault(u => u.Login.Equals(auth.Login));
                    return user != null && user.PassWord.Equals(auth.Password);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    internal static class Extension
    {
        public static IPAddress GetNext(this IPAddress ipAddress, uint increment=1)
        {
            byte[] addressBytes = ipAddress.GetAddressBytes().Reverse().ToArray();
            uint ipAsUint = BitConverter.ToUInt32(addressBytes, 0);
            var nextAddress = BitConverter.GetBytes(ipAsUint + increment);
           return IPAddress.Parse(String.Join(".", nextAddress.Reverse()));
        }
    }
}
