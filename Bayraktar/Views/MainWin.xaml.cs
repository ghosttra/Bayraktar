using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using BayraktarClient;
using BayraktarGame;

namespace Bayraktar
{
    /// <summary>
    /// Interaction logic for MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        public MainWin()
        {
            InitializeComponent();
            try
            {
                CurrentClient.Instance.Init();
                CurrentClient.Instance.Client.Disconnect+=Disconnect;
                CurrentClient.Instance.Client.StartGame+=  StartGame;
                CurrentClient.Instance.Client.WaitForGame+= WaitForGame;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return;
            }
            Content = new Authorize();
        }

        private void Disconnect()
        {
            _invoke(() =>
            {
                new MessageBox("Client has been disconnected by server") { Owner = this }.ShowDialog();
                CurrentClient.Instance.Client.Refresh();
                if (Content is Authorize) return;
                Content = new Authorize();
            });
        }


        private void _initWaitingBox()
        {
            var cancel = new Button
            {
                Content = "Cancel"
            };
            cancel.Click += (sender, args) =>
            {
                CurrentClient.Instance.StopWaitingForGame();
                _waitingBox.Close();
            };
            _waitingBox = new MessageBox("Wait for game", new List<Button> { cancel }){Owner = this};
        }

        private MessageBox _waitingBox;
        private void WaitForGame(bool isWait)   
        {
            if (isWait)
            {
                _invoke(() =>
                {
                    _initWaitingBox();
                    _waitingBox.Show();
                });
            }
            else
            {
                _invoke(()=>_waitingBox.Close());
            }
        }
        private void _invoke(Action action)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(action);
            else
                action();
        }

        private void StartGame(GameClient client, GameRole role)
        {
           _invoke(() =>
           {
            var game = new Game(client, role);
            Content = game;
           });
           
        }
        
        private void MainWin_OnClosing(object sender, CancelEventArgs e)
        {
            CurrentClient.Instance.Client.Close();
        }
    }
}
