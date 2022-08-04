using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Bayraktar
{

    public partial class Game : Window
    {
        List<AnimationClock> clocks = new List<AnimationClock>();
        private void Cycle()
        {
            DoubleAnimation DA = new DoubleAnimation
            {
                From = -300,
                To = SystemParameters.PrimaryScreenHeight + 300,
            };
            short seconds = (short)rnd.Next(5, 15);
            DA.Duration = TimeSpan.FromSeconds(seconds);
            //short amountOfUnits = short.Parse(rnd.Next(1, 3).ToString());
            short left = 0;
            for (short i = 0; i < 3; i++)
            {
                MilitaryUnit militaryU = new MilitaryUnit("APC-Z");
                Canvas.SetLeft(militaryU, left);
                Canvas.SetTop(militaryU, -300);
                AnimationClock clock;
                clock = DA.CreateClock();
                clocks.Add(clock);
                militaryU.MouseLeftButtonUp += MilitaryUnit_MouseLeftButtonUp;
                militaryU.ApplyAnimationClock(Canvas.TopProperty, clock);
                var this_ = (CMUs.Children[i] as Border).Child;
                (this_ as Canvas).Children.Add(militaryU);
            }
        }

        Random rnd = new Random();
        public Game()
        {
            InitializeComponent();

            Cursor = new Cursor(System.IO.Path.GetFullPath(@"../Data/Pictures/curOfBayraktar.cur"));

            var imgBr = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\Data\Pictures\Levels\Road.png", UriKind.Relative)) };
            Pause.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\pause.png") as ImageSource;

            Road.Background = imgBr;
            Road1.Background = imgBr;
            Road2.Background = imgBr;

            Cycle();
        }
        private async void MilitaryUnit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (sender as MilitaryUnit).BeginAnimation(Canvas.TopProperty, new DoubleAnimation() { To = Canvas.GetTop((sender as MilitaryUnit)) });

            await Task.Delay(TimeSpan.FromSeconds(5));
            CMU.Children.Remove((UIElement)sender);
        }

        private void PauseAnimation()
        {
            foreach (var clock in clocks)
            {
                clock.Controller.Pause();
            }
        }
        private void ResumeAnimation()
        {
            foreach (var clock in clocks)
            {
                clock.Controller.Resume();
            }
        }
        private void Pause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rect.Visibility = Visibility.Visible;
            PauseAnimation();
            PauseWindow pauseWindow = new PauseWindow();
            pauseWindow.ShowDialog();
            if (pauseWindow.DialogResult.HasValue && pauseWindow.DialogResult.Value)
                Close();
            ResumeAnimation();
            Rect.Visibility = Visibility.Hidden;
        }
    }
}