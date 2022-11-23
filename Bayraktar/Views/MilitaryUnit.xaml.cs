using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BayraktarGame;

namespace Bayraktar
{
    public partial class MilitaryUnit : UserControl
    {
        public Unit _unit { get; set; }
        public MilitaryUnit(Unit unit)
        {
            this._unit = unit;
            InitializeComponent();
            _setImageSource(UnitImg, _unit.Image);

            this.UnitImg.MouseLeftButtonUp += MU_MouseLeftButtonUp;

            _setImageSource(UnitImg, _unit.Image);

            threadMove = new Thread(f => {
                while (true) {
                    X += 1;
                    Thread.Sleep(5);
                    this.Dispatcher.Invoke(() => {
                        Canvas.SetTop(this, X);
                    });
                    if (X > SystemParameters.PrimaryScreenHeight + 250) {
                        Stop();
                        break;
                    }
                }
            });
        }
        DispatcherTimer timer;
        private void MU_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            _setImageSource(UnitImg, _unit.ImageDestroyed);
            UnitImg.Source = new ImageSourceConverter().ConvertFromString(@"C:\Users\Макс\source\repos\Bayraktar\Bayraktar\Pictures\MilitaryUnits\APC-Z_Destroyed.png") as ImageSource;
            (sender as Image).MouseLeftButtonUp -= MU_MouseLeftButtonUp;
            Stop();
        }
        private void Timer_Tick(object sender, EventArgs e) {
            MUGrid.Children.Remove(UnitImg);
            timer.Stop();
        }
        public int X { get; set; } = -300;
        public int Y { get; set; }
        Thread threadMove;
        public void Move() {
            threadMove.Start();
        }

        public void Stop() {
            threadMove.Abort();
        }

        public DoubleAnimation Animation;
       
        private void _setImageSource(Image target, byte[] source)
        {
            if (source == null)
                return;
            target.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(source);
        }
        //private void MU_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    Destroy();
        //    ((Image)sender).MouseLeftButtonUp -= MU_MouseLeftButtonUp;
        //}

        public Action<MilitaryUnit> IsDestroyed;
        public void Destroy()
        {
            _setImageSource(UnitImg, _unit.ImageDestroyed);
            
            IsDestroyed?.Invoke(this);
        }
    }
}
