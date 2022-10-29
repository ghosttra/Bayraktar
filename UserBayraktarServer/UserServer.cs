using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Message;

namespace UserBayraktarServer
{
    public class UserServer
    {
        private TcpListener _server;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        public event Action<string> Inform;

        public UserServer(IPAddress address, int port)
        {
            _server = new TcpListener(address, port);
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            Stopped += () => Inform?.Invoke("Server Stopped");
        }
        public Task StartAsync() => Task.Factory.StartNew(Start, _token);
        public void Start()
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
                        Task.Factory.StartNew(_accept, _token);
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
            UserConnection clientConnection = new UserConnection(client);
            var auth = clientConnection.Authorize();
            if (auth == null) return;
            if (_authorize(auth))
            {

            }
        }

        private bool _authorize(MessageAuthorize auth)
        {

            return false;
        }
    }

    public class UserConnection
    {
        private TcpClient _client;
        private NetworkStream _stream => _client.GetStream();

        private readonly CancellationTokenSource _cts;
        private readonly CancellationToken _token;
        public UserConnection(TcpClient client)
        {
            _client = client;
            _isRun = true;
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        private bool _isRun;
        public Task RunAsync()=>Task.Factory.StartNew(Run, _token);

        public void Run()
        {
            while (_isRun)
            {

            }
        }

        public void Close()
        {
            _cts.Cancel();
            _isRun = false;
        }

        private MessagePacket _read()
        {
            var buffer = new byte[1024];
            do
            {
                _stream.Read(buffer, 0, buffer.Length);
            } while (_stream.DataAvailable);
            return MessagePacket.FromBytes(buffer);
        }
        public MessageAuthorize Authorize()
        {
            if (_read() is MessageAuthorize authorize)
            {
                return authorize;
            }

            return null;
        }
    }
}
