using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BayraktarGame;
using Message;

namespace UserBayraktarClient
{
    public class UserClient
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                if(IsConnected!=true)
                    _user = value;
                else
                {
                    throw new InvalidOperationException("User is connected and can't be changed");
                }
            } }
        private IPAddress _serverIp;
        private int _serverPort;
        private TcpClient _client;
        private NetworkStream _stream=> _client.GetStream();
        public bool? IsConnected => _client?.Connected;
        public UserClient(string serverIp, int serverPort, User user) :this(serverIp, serverPort)
        {
            User= user;
        }

        public UserClient(string serverIp, int serverPort) : this(IPAddress.Parse(serverIp),serverPort)
        {
        }

        public UserClient(IPAddress serverIp, int serverPort)
        {
            _client = new TcpClient();
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public event Action<string> Info;
        public event Action Connected;
        public event Action Disconnected;

        public Task ConnectAsync() => Task.Factory.StartNew(Connect);
        public async void Connect()
        {
            try
            {
                if (User == null)
                    throw new ArgumentNullException("No user to authtorize");
                await _client.ConnectAsync(_serverIp, _serverPort);
                if (!_authorize())
                {
                    Close();
                    throw new ArgumentException("Authorize error");
                }
                Connected?.Invoke();
            }
            catch (Exception e)
            {
                Info?.Invoke(e.Message);
            }
        }

        public void Close()
        {
            _client.Close();
            Disconnected?.Invoke();
        }
        private bool _authorize()
        {
            MessageAuthorize authorize = new MessageAuthorize(AuthorizeMode.Login)
            {
                Login = User.Login, Password = User.PassWord
            };
            Send(authorize);
            if (_read() is MessageBool response)
            {
                return response.Response;
            }
            return false;
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
        public void Receive()
        {
            var message = _read();
            switch (message)
            {
                case MessageCommand command:

                    break;
                case null:
                    throw new NullReferenceException("Can't read message");
            }
        }
        public void Send(MessagePacket message)
        {
            var buffer = message.ToBytes();
            _stream.Write(buffer, 0, buffer.Length);
        }
    }
}
