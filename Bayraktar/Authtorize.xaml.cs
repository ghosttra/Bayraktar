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
                new MessageBox(e.Message) { Owner = Parent as Window }.ShowDialog();
            }
        }

        private void _init()
        {
            _userClient = CurrentClient.Instance.Client;
            _userClient.Info += _userClient_Info;
            _subscribe();
        }

        private void _subscribe()
        {
            _userClient.Connected += _userClient_Connected;
        }


        private void _unsubscribe()
        {
            _userClient.Connected -= _userClient_Connected;
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
            _invoke(() => new MessageBox(info) { Owner = Parent as Window }.ShowDialog());
        }

        private void _userClient_Disconnected()
        {
        }

        private void _userClient_Connected(bool connect)
        {
            _invoke(() => { Login.IsEnabled = true; });
            if (!connect) return;
            _unsubscribe();
            _invoke(() => ((Window)Parent).Content = new MainMenu());

        }

        private async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_check())
                return;
            _boxReset();
            _userClient.User = _getUserData();
            Login.IsEnabled = false;
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

        private User _getUserData() => new User
        {
            Login = LoginBox.Text,
            PassWord = PassWordBox.Password.GetHashCode().ToString()
        };

        private bool _check()
        {
            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                LoginBox.BorderBrush = Brushes.DarkRed;
                return false;
            }

            if (string.IsNullOrEmpty(PassWordBox.Password))
            {
                PassWordBox.BorderBrush = Brushes.DarkRed;
                return false;
            }

            return true;
        }

        private void _boxReset()
        {
            LoginBox.BorderBrush = Brushes.AliceBlue;
            PassWordBox.BorderBrush = Brushes.AliceBlue;

        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            //todo
            if (!_check())
            {
                new MessageBox("Registration failed");
                return;
            }

            User user = _getUserData();

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

    public class CurrentClient
    {
        public UserClient Client { get; private set; }

        private CurrentClient()
        {
        }

        private static CurrentClient _instance;

        public static CurrentClient Instance => _instance ?? (_instance = new CurrentClient());

        public void Init()
        {
            try
            {

                var env = DotEnv.Read();
                var ip = IPAddress.Parse(env["SERVER_IP"].Trim());
                var port = Int32.Parse(env["SERVER_PORT"]);
                Client = new UserClient(ip, port);
            }
            catch
            {
                // ignored
            }
        }
    }
}
