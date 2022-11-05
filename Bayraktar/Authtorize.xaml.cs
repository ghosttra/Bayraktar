using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BayraktarGame;
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


        private void _userClient_Connected(bool connect)
        {
            _invoke(() =>
            {
                ButtonPanel.IsEnabled = true;
            });
            if (!connect) return;
            _unsubscribe();
            _invoke(() => ((Window)Parent).Content = new MainMenu());

        }

        private async void _connect(bool isLogin)
        {
            if (!_check())
                return;
            _boxReset();
            _userClient.User = _getUserData();

            ButtonPanel.IsEnabled = false;
            try
            {
                if (isLogin)
                    await _userClient.ConnectAsync();
                else
                    await _userClient.ConnectAsyncR();
            }
            catch (Exception exception)
            {
                _userClient.Refresh();
                new MessageBox(exception.Message).ShowDialog();
            }
        }

        private async void Registration_Click(object sender, RoutedEventArgs e)
        {
            _connect(false);
        }

        private async void Login_OnClick(object sender, RoutedEventArgs e)
        {

            _connect(true);
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
