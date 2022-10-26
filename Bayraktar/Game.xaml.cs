using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BayraktarGame;
using GameEntities;

namespace Bayraktar
{
    public partial class Game : Window
    {

        public Game(bool gameMode) : this()
        {
            _gameMode = gameMode;
        }

        //attack or defense
        private bool _gameMode;
        public User User { get; set; }
        public short MaxSpeed { get; set; } = 5;
        public short MinSpeed { get; set; } = 15;
        public int HealthPoints { get; set; }
        public int Score { get; set; } = 0;
        List<AnimationClock> clocks = new List<AnimationClock>();
        private void AddMilitaryUnit()
        {
            DoubleAnimation DA = new DoubleAnimation
            {
                From = -300,
                To = SystemParameters.PrimaryScreenHeight + 250,
            };

            if (MinSpeed >= 10)
                MinSpeed -= (short)rnd.Next(1, 3);
            if (MaxSpeed >= 5)
                MaxSpeed -= (short)rnd.Next(1, 3);

            if (MinSpeed > MaxSpeed)
            {
                (MinSpeed, MaxSpeed) = (MaxSpeed, MinSpeed);
            }

            short seconds = (short)rnd.Next(MinSpeed, MaxSpeed);

            DA.Duration = TimeSpan.FromSeconds(seconds);
            short left = (short)rnd.Next(-50, 50);

            //todo
            MilitaryUnit militaryU = new MilitaryUnit(new Unit());
            Canvas.SetLeft(militaryU, left);
            Canvas.SetTop(militaryU, -300);
            militaryU.Width = Road.Width / 3;
            militaryU.Height = Road.Width / 3;
            AnimationClock clock;
            DA.Completed += (s, e) =>
            {
                HealthPoints--;
                if (HealthPoints == 0)
                {
                    PauseFunc("Поразка", true);
                }
                if (HealthPoints <= 5)
                {
                    HealthText.Content = "Health: " + HealthPoints.ToString();
                }

            };
            clock = DA.CreateClock();
            clocks.Add(clock);
            militaryU.MouseLeftButtonUp += MilitaryUnit_MouseLeftButtonUp;
            militaryU.ApplyAnimationClock(Canvas.TopProperty, clock);
            int temp = rnd.Next(0, CMUs.Children.Count);
            while (!(CMUs.Children[temp] is Border))
            {
                temp = rnd.Next(0, 3);
            }
            var this_ = (CMUs.Children[temp] as Border).Child;
            (this_ as Canvas).Children.Add(militaryU);

        }


        Random rnd = new Random();
        DispatcherTimer dispatcherTimer;
        //private Timer _clockTimer;

        public Game()
        {
            InitializeComponent();

            //_clockTimer = new Timer(1000);
            //_clockTimer.Elapsed += clockTimer_Elapsed;
            //_clockTimer.Start();
            DataContext = this;

            Cursor = new Cursor(System.IO.Path.GetFullPath(@"../Data/Pictures/curOfBayraktar.cur"));
            //Pause.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\pause.png") as ImageSource;

            Canvas.SetTop(HealthText, SystemParameters.PrimaryScreenHeight - 50);
            //Canvas.SetTop(RandomText1, SystemParameters.PrimaryScreenHeight - 50);

            dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            AddMilitaryUnit();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            for (int r = 0; r < rnd.Next(3, 9); r++)
            {
                AddMilitaryUnit();
            }
        }

        private async void MilitaryUnit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is MilitaryUnit)
            {
                var MU = (sender as MilitaryUnit);
                MU.BeginAnimation(Canvas.TopProperty, new DoubleAnimation() { To = Canvas.GetTop(MU) });
                Score += rnd.Next(50, 200);

                MU.MouseLeftButtonUp -= MilitaryUnit_MouseLeftButtonUp;

                MU.IsHitTestVisible = false;

                HealthPoints++;

                lblScore.Content = "Score: " + Score.ToString();

                await Task.Delay(TimeSpan.FromSeconds(3));
                // CMU.Children.Remove((UIElement)sender);

            }

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

        private void PauseFunc(string title, bool isDefeat)
        {
            Rect.Visibility = Visibility.Visible;
            dispatcherTimer.Stop();
            PauseAnimation();
            PauseWindow pauseWindow = new PauseWindow(title, isDefeat);
            pauseWindow.ShowDialog();
            if (pauseWindow.DialogResult.HasValue && pauseWindow.DialogResult.Value)
                Close();
            else
            {
                ResumeAnimation();
                Rect.Visibility = Visibility.Hidden;
                dispatcherTimer.Start();
            }

        }

        private void _pause()
        {
            PauseFunc("На паузі", false);
        }
        private void Pause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _pause();
        }

        private void Pause_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && e.IsRepeat == false)
                _pause();
        }

        private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveResult saveResult = new SaveResult();
            saveResult.ShowDialog();
            this.User = new User() { Name = Environment.UserName, TimeOf_AtteptEnd = DateTime.Now, Score = this.Score };
            if (!string.IsNullOrWhiteSpace(saveResult.Name))
                User.Name = saveResult.Name;
        }
    }
}