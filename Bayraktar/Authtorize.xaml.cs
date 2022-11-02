using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BayraktarGame;
using dotenv.net;
using UserGameClient;

namespace Bayraktar
{
    public partial class Authorize : UserControl
    {
        private UserClient _userClient;
        public Authorize()
        {
            InitializeComponent();
            Focus();
            try
            {
                _init();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        private void _init()
        {
            DotEnv.Load();
            var env = DotEnv.Read();
            var ip = IPAddress.Parse(env["SERVER_IP"].Trim());
            var port = Int32.Parse(env["SERVER_PORT"]);
            _userClient = new UserClient(ip, port);
            _userClient.Info += (info) =>
            {
                _invoke(() => _userClient_Info(info));
            };
            _userClient.Connected += ()=>_invoke(_userClient_Connected);
            _userClient.Disconnected += ()=> _invoke(_userClient_Disconnected);
        }

        private void _invoke(Action action)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(action);
            else
                action();
        }

        private void _userClient_Info(string info)
        {
            var mBox = new MessageBox(info)
            {
                Owner = (Parent as Window)
            };
            mBox.ShowDialog();
        }

        private void _userClient_Disconnected()
        {

        }

        private void _userClient_Connected()
        {
            ((Window)Parent).Content = new MainMenu();
        }

        private async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            _userClient.User = new User
            {
                Login = LoginBox.Text,
                PassWord = PassWordBox.Password
            };
            try
            {
                await _userClient.ConnectAsync();
            }
            catch (Exception exception)
            {
                _userClient.Refresh();
                new MessageBox(exception.Message).ShowDialog();
            }

        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void Authorize_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
