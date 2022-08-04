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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bayraktar
{
    public partial class MilitaryUnit : UserControl
    {
        private string Unit;
        public MilitaryUnit(string Unit)
        {
            this.Unit = Unit;
            InitializeComponent();
            MU.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\MilitaryUnits\" + Unit + ".png") as ImageSource;
        }

        private void MU_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            MU.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\MilitaryUnits\" + Unit + "_Destroyed.png") as ImageSource;
        }

        public void stopAnimation()
        {
            MU.BeginAnimation( Grid.WidthProperty, new DoubleAnimation());
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MUGrid.Children.Remove(MU);
        }
    }
}
