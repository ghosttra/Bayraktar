using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BayraktarGame;

namespace Bayraktar {
    public partial class MilitaryUnit : UserControl {
        public Unit Unit { get; set; }
        public MilitaryUnit(Unit unit) {
            this.Unit = unit;
            InitializeComponent();

            MU.Source = new ImageSourceConverter().ConvertFromString(@"C:\Users\Макс\source\repos\Bayraktar\Bayraktar\Pictures\MilitaryUnits\APC-Z.png") as ImageSource;

            this.MU.MouseLeftButtonUp += MU_MouseLeftButtonUp;

            _setImageSource(MU.Source, Unit.Image);

            threadMove = new Thread(f => {
                while (true) {
                    X += 1;
                    Thread.Sleep(5);
                    this.Dispatcher.Invoke(() =>
                    {
                        Canvas.SetTop(this, X);
                    });
                    if (X > SystemParameters.PrimaryScreenHeight + 250) {
                        // отнимаем хп
                        break;
                    }
                }
                Stop();
            });
        }

        private void _setImageSource(ImageSource target, byte[] source) {
            if (source == null)
                return;
            var image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(source)) {
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
            }
            MU.Source = image;
        }
        DispatcherTimer timer;
        private void MU_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            _setImageSource(MU.Source, Unit.ImageDestroyed);
            MU.Source = new ImageSourceConverter().ConvertFromString(@"C:\Users\Макс\source\repos\Bayraktar\Bayraktar\Pictures\MilitaryUnits\APC-Z_Destroyed.png") as ImageSource;
            (sender as Image).MouseLeftButtonUp -= MU_MouseLeftButtonUp; 
            Stop();
        }

        private void Timer_Tick(object sender, EventArgs e) {
            MUGrid.Children.Remove(MU);
            timer.Stop();
        }
        public int X { get; set; } = -300;
        public int Y { get; set; }
        public bool isDestroyed { get; set; }
        Thread threadMove;
        public void Move() {
            threadMove.Start();
        }

        public void Stop() {
            threadMove.Abort();
        }
    }
}
