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
            var clientConnection = new UserConnection(client);
            var auth = clientConnection.Authorize();
            if (auth == null) clientConnection.Close();
            var result = _authorize(auth);
            clientConnection.Send(new MessageBool(result));
            if (!result)
            {
                clientConnection.Close();
            }
            clientConnection.Query += Query;
            clientConnection.RunAsync();
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
                }
            }
        }

        #region Commands

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
                    return true;
                case AuthorizeMode.Login:
                    var user = _context.Users.FirstOrDefault(u => u.Login.Equals(auth.Login));
                    return user != null && user.PassWord.Equals(auth.Password);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
