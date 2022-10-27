using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BayraktarGame;

namespace BayraktarClient
{
    public class GameClient
    {
        private IPEndPoint _server;
        private int _localPort;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        public short MaxSpeed { get; set; } = 5;
        public short MinSpeed { get; set; } = 15;
        public int HealthPoints { get; set; }
        public GameClient(int localPort, IPEndPoint server)
        {
            _localPort = localPort;
            _server = server;
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _receive();
                }
            }, _token);
        }

        private void _send()
        {

        }

        private void _receive()
        {

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
