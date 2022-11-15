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
                CurrentClient.Instance.StartGame+=  StartGame;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return;
            }
            Content = new Authorize();
        }

        private void StartGame(GameClient client, GameRole role)
        {
            Content = new Game(client,role);
        }
        
        private void MainWin_OnClosing(object sender, CancelEventArgs e)
        {
            CurrentClient.Instance.Client.Close();
        }
    }
}
