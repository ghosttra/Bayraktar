using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameServerInterface;
using UserBayraktarServer;

namespace GameServerInterface
{
    public partial class ServerInterface : Form
    {
        public ServerInterface()
        {
            InitializeComponent();
        }

        private void editorBtn_Click(object sender, EventArgs e)
        {
            new UnitsEdit().ShowDialog();
        }

        private UserServer _server;
        private void startBtn_Click(object sender, EventArgs e)
        {
            _server = new UserServer(IPAddress.Parse(serverIpBox.Text), decimal.ToInt32(portBox.Value));
            _server.Inform += (info) =>
            {
                if (InvokeRequired)
                    Invoke(new Action((() => _server_Inform(info))));
                else
                {
                    _server_Inform(info);
                }
            };
            _server.Stopped += () =>
            {
                if (InvokeRequired)
                    Invoke(new Action(_server_Stopped));
                else
                {
                    _server_Stopped();
                }
            };
            _server.StartAsync();
            _switch();
        }

        private void _server_Stopped()
        {
            _switch();
        }

        private void _server_Inform(string info)
        {
            logBox.Items.Add(info);
        }

        private void _switch()
        {
            (startBtn.Enabled, stopBtn.Enabled) = (stopBtn.Enabled, startBtn.Enabled);
            configPanel.Enabled =!configPanel.Enabled;
        }

        private void userControlBtn_Click(object sender, EventArgs e)
        {
            new UsersControl().ShowDialog();
        }

        private void ServerInterface_Load(object sender, EventArgs e)
        {
            startBtn.PerformClick();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            _server.Stop();
        }
    }
}
