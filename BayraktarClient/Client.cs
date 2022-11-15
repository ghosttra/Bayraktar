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
        private int _localPort;
        public User User { get; }
        private CancellationTokenSource _cts;
        private CancellationToken _token;

        //public short MaxSpeed { get; set; } = 5;
        //public short MinSpeed { get; set; } = 15;
        //public int HealthPoints { get; set; }

        public GameClient(User user, int localPort, IPEndPoint server)
        {
            User = user;
            _localPort = localPort;
            _server = server;
            _client = new UdpClient(localPort);
            _client.JoinMulticastGroup(_server.Address, 15);
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            Task.Factory.StartNew(_start, _token);
        }

        private void _start()
        {
            while (true)
            {
                try
                {
                    _receive();
                }
                catch (SocketException e)
                {
                    _localPort++;
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
           var receive =  new UdpClient(_localPort).ReceiveAsync();
           _handle(receive.Result);
        }

        private void _handle(UdpReceiveResult receiveResult)
        {
            var buffer = receiveResult.Buffer;
            
        }

        public void GetAllUnits()
        {

        }

        public void SetUnit(Unit unit, int x, int y)
        {

        }

        public void Shoot(int x, int y)
        {

        }

    }
}
