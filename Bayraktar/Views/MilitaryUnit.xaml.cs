using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BayraktarGame;

namespace Bayraktar
{
    public partial class MilitaryUnit : UserControl
    {
        private Unit Unit;
        public MilitaryUnit(Unit unit)
        {
            this.Unit = unit;
            InitializeComponent();
            _setImageSource(MU, unit.Image);
            //MU.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\MilitaryUnits\" + unit + ".png") as ImageSource;
        }

        private void _setImageSource(Image target, byte[] source)
        {
            if(source == null)
                return;
            using (var stream = new MemoryStream(source))
            {
                target.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(source);
            }
            //using (MemoryStream stream  = new MemoryStream(source))
            //{
            //    var image = new BitmapImage();
            //    image.BeginInit();
            //    image.StreamSource = stream;
            //    image.EndInit();
            //    image.Freeze();
            //}
        }
        DispatcherTimer timer;
        private void MU_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            _setImageSource(MU, Unit.ImageDestroyed);
            //MU.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\MilitaryUnits\" + Unit + "_Destroyed.png") as ImageSource;
            (sender as Image).MouseLeftButtonUp -= MU_MouseLeftButtonUp;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MUGrid.Children.Remove(MU);
            timer.Stop();
        }
    }
}
