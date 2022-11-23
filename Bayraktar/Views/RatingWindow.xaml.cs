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
using Bayraktar;
using BayraktarGame;
using Message;

namespace Bayraktar {


    public partial class RatingWindow : UserControl {
        public RatingWindow() {
            InitializeComponent();
            _init();
        }

        private void _init() {
            RatingControl.Instance.Result += SetStats;
            RatingControl.Instance.NoData += NoData;
            _updateRating();
        }

        private void NoData() {
            //todo
        }

        private void SetStats(List<Statistic> rating) {
            _invoke(() => {
                RatingsLB.ItemsSource = null;
                RatingsLB.ItemsSource = rating;
            });
        }

        private void _invoke(Action action) {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(action);
            else
                action();
        }

        
        private void Exit(object sender, RoutedEventArgs e) {
            _exit();
        }

        private void RatingWindow_OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape)
                _exit();
        }

        private void _exit() {
            RatingControl.Instance.Result -= SetStats;
            ((Window)Parent).Content = new MainMenu();
        }

        private RatingSort _sort = RatingSort.Date;
        private RatingType _type = RatingType.All;
        private void SortBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) return;

            switch (button.Tag.ToString()) {
                case "Score":
                    _sort = RatingSort.Score;
                    break;
                case "Date":
                    _sort = RatingSort.Date;
                    break;
                case "My":
                    _type = RatingType.User;
                    break;
                case "All":
                    _type = RatingType.All;
                    break;
            }

            _updateRating();
        }

        private void _updateRating()
        {
            RatingControl.Instance.GetRating(_type, _sort);
        }
    }
}

