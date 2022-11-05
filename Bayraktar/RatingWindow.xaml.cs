using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using Message;

namespace Bayraktar
{
    public partial class RatingWindow : UserControl
    {
        public RatingWindow()
        {
            InitializeComponent();
            _init();
        }

        private void _init()
        {
            RatingsLB.ItemsSource = _getStats().ToList();
        }

        private IEnumerable<Statistic> _getStats()
        {
            var message = new MessageCommand
            {
                Command = "RATING"
            };
            CurrentClient.Instance.Send(message);
            try
            {
                var ratings = CurrentClient.Instance.Receive();
                if (ratings is MessageDataContent data)
                {
                    return _loadRating(data.Content);
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private List<Statistic> _loadRating(byte[] dataContent)
        {
            using (MemoryStream stream = new MemoryStream(dataContent))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (formatter.Deserialize(stream) is List<Statistic> statistics)
                {
                    return statistics;
                }
                return null;
            }
        }

        private void SortByScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            RatingsLB.ItemsSource = null;
            RatingsLB.ItemsSource = _getStats().OrderBy(s => s.Score);
        }

        private void ShowMy_Click(object sender, RoutedEventArgs e)
        {
            RatingsLB.ItemsSource = null;
            RatingsLB.ItemsSource = _getStats().Where(s => s.User?.Login.Equals(CurrentClient.Instance.Client.User.Login)==true);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            _exit();
        }

        private void RatingWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                _exit();
        }

        private void _exit()
        {
            ((Window)Parent).Content = new MainMenu();
        }
    }
}
