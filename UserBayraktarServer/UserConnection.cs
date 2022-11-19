using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BayraktarGame;
using Message;

namespace UserBayraktarServer
{
    public class UserConnection
    {
        private TcpClient _client;
        private NetworkStream _stream => _client.GetStream();
        public Action<UserConnection, bool> InQueueEvent;
        private bool _inQueue = false;
        public bool InQueue
        {
            get => _inQueue;
            private set
            {
                _inQueue = value;
                InQueueEvent?.Invoke(this, value);
            }
        } //wait for multiplayer game

        public void WaitingForMultiplayer()
        {
            InQueue = true;
        }

        public void StopWaitingFoeMultiplayer()
        {
            InQueue = false;
        }
        
        private readonly CancellationTokenSource _cts;
        private readonly CancellationToken _token;
        public User User { get; set; }
        public UserConnection(TcpClient client)
        {
            _client = client;
            _isRun = true;
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        private bool _isRun;
        public Task RunAsync() => Task.Factory.StartNew(Run, _token);

        public void Run()
        {
            while (_isRun)
            {
                Receive();
            }
        }

        public Action<UserConnection, MessagePacket> Query;
        public Action<UserConnection> CloseConnection;
        private void Receive()
        {
            try
            {
                var message = _read();
                if (message == null) return;
                Query?.Invoke(this, message);
            }
            catch (Exception e)
            {
                Close();
            }
        }

        public void Close()
        {
            _cts.Cancel();
            _isRun = false;
            _client.Close();
            CloseConnection?.Invoke(this);
        }

        public void Disconnect()
        {
            MessageCommand command = new MessageCommand
            {
                Command = "DISCONNECT"
            };
            Send(command);
            Close();
        }

        private MessagePacket _read()
        {

            var buffer = new byte[2048];
            using (var stream = new MemoryStream())
            {
                do
                {
                    var bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, bytesRead);
                } while (_stream.DataAvailable);
                return MessagePacket.FromBytes(stream.ToArray());
            }
        }

        public void Send(MessagePacket message)
        {
            var buffer = message.ToBytes();

            //var length = new MessageDataContent(buffer.Length);
            //var lengthBuffer = length.ToBytes();
            //_stream.Write(lengthBuffer, 0, lengthBuffer.Length);
            //_stream.Read(lengthBuffer, 0, lengthBuffer.Length);

            _stream.Write(buffer, 0, buffer.Length);
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