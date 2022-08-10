using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Bayraktar
{
    public partial class MilitaryUnit : UserControl
    {
        private string Unit;
        public bool isDestroyed { get; set; } = false;
        public MilitaryUnit(string Unit)
        {
            this.Unit = Unit;
            InitializeComponent();
            MU.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\MilitaryUnits\" + Unit + ".png") as ImageSource;
        }
        DispatcherTimer timer;
        private void MU_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            MU.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\MilitaryUnits\" + Unit + "_Destroyed.png") as ImageSource;
            (sender as Image).MouseLeftButtonUp -= MU_MouseLeftButtonUp;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MUGrid.Children.Remove(MU);
            timer.Stop();
        }
    }
}
