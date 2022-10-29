using System;
using System.Collections.Generic;
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
using BayraktarGame;

namespace Bayraktar
{
    /// <summary>
    /// Логика взаимодействия для RatingWindow.xaml
    /// </summary>
    public partial class RatingWindow : UserControl
    {
        public List<User> users { get; set; } = new List<User>();
        public RatingWindow(List<User> users)
        {
            InitializeComponent();
            RatingsLB.ItemsSource = users;
            this.users = users;
        }

        private void SortByScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            RatingsLB.ItemsSource = null;
           // users.Sort((x, y) => y.Score.CompareTo(x.Score));
            RatingsLB.ItemsSource = users;
        }

        private void SortByDateBtn_Click(object sender, RoutedEventArgs e)
        {
            RatingsLB.ItemsSource = null;
           // users.Sort((x, y) => y.TimeOf_AtteptEnd.CompareTo(x.TimeOf_AtteptEnd));
            RatingsLB.ItemsSource = users;
        }
    }
}
