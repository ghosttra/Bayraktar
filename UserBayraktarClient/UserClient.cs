using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BayraktarGame;
using Message;

namespace UserGameClient
{
    public class UserClient
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                if (IsConnected != true)
                    _user = value;
                else
                {
                    throw new InvalidOperationException("User is connected and can't be changed");
                }
            }
        }
        private IPAddress _serverIp;
        private int _serverPort;
        private TcpClient _client;
        private NetworkStream _stream => _client.GetStream();
        public bool? IsConnected => _client?.Connected;
        public UserClient(string serverIp, int serverPort, User user) : this(serverIp, serverPort)
        {
            User = user;
        }

        public UserClient(string serverIp, int serverPort) : this(IPAddress.Parse(serverIp), serverPort)
        {
        }

        public UserClient(IPAddress serverIp, int serverPort)
        {
            _client = new TcpClient();
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public event Action<string> Info;
        public event Action<bool> Connected;

        private async void _connect(Func<bool> authorization)
        {
            try
            {
                if (User == null)
                    throw new ArgumentNullException("No user to authtorize");
                await _client.ConnectAsync(_serverIp, _serverPort);
              
                    var auth = authorization();
                    Connected?.Invoke(auth);
                    if (!auth)
                    {
                        Close();
                        _client = new TcpClient();
                    }
            }
            catch (Exception e)
            {
                Connected?.Invoke(false);
                Info?.Invoke(e.Message);
            }
        }

        public Task ConnectAsync() => Task.Factory.StartNew(Connect);
        public void Connect()
        {
            _connect(()=>_authorize(true));
        }

        //R = Registration
        public void ConnectR()
        {
            _connect(() => _authorize(false));
        }
        public Task ConnectAsyncR()=> Task.Factory.StartNew(ConnectR);

        public void Close()
        {
            _client.Close();
            Connected?.Invoke(false);
        }
        private bool _authorize(bool isLogin)
        {
            var mode = isLogin ? AuthorizeMode.Login : AuthorizeMode.Registration;
            var authorize = new MessageAuthorize(mode)
            {
                Login = User.Login,
                Password = User.PassWord
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
        public MessagePacket Receive()=>_read();
            
        public void Send(MessagePacket message)
        {
            var buffer = message.ToBytes();
            _stream.Write(buffer, 0, buffer.Length);
        }

        public void Refresh()
        {
            _client = new TcpClient();
        }

    }
}
