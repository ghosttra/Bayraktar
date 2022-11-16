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
using BayraktarClient;
using BayraktarGame;
using Message;

namespace Bayraktar
{

    public partial class Game : UserControl
    {

        //Attack or Defense
        private GameRole _gameRole;
        private GameClient _client;

        public User User { get; set; }

        public Game(GameClient client, GameRole gameRole) : this()
        {
            _client = client;
            _client.HealthChanged += HealthChanged;
            _client.ScoreChanged += ScoreChanged;
            _client.GameOver += GameOver;
            _client.SetUnitAction += SetUnitAction;
            _gameRole = gameRole;
            switch (gameRole)
            {
                case GameRole.Attack:
                    //todo
                    //интерфейс атаки
                    break;
                case GameRole.Defense:
                    //todo
                    //интерфейс обороны
                    break;
            }

            //dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
            //dispatcherTimer.Tick += (sender, args) =>
            //{
            //    for (int r = 0; r < rnd.Next(3, 9); r++)
            //    {
            //        AddMilitaryUnit();
            //    }
            //};
            //dispatcherTimer.Start();

        }

        private void SetUnitAction(MessageUnit unitData)
        {
            _invoke(()=>_setUnit(unitData));
        }

        private void _setUnit(MessageUnit unitData)
        {
            ///////////////////////////////////////////////
            ///
            ///установить юнита на игровое поле и запустить анимацию
            ///
            ///
            
            DoubleAnimation DA = new DoubleAnimation
            {
                From = -300,
                To = SystemParameters.PrimaryScreenHeight + 250,
            };
            MilitaryUnit militaryU = new MilitaryUnit(unitData.Unit);
            Canvas.SetLeft(militaryU, unitData.Coords.X);
            Canvas.SetTop(militaryU, -300);
            militaryU.Width = Road.Width / 3;
            militaryU.Height = Road.Width / 3;
            AnimationClock clock;
            DA.Completed += (s, e) =>
            {
                //HealthPoints--;
                //if (HealthPoints == 0)
                //{
                //    PauseFunc("Поразка", true);
                //}
                //if (HealthPoints <= 5)
                //{
                //    HealthText.Content = "Health: " + HealthPoints.ToString();
                //}
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
        private void ScoreChanged(int score)
        {
            //пример
    
            _invoke(() => lblScore.Content = score);
        }
        private void HealthChanged(int hp)
        {
            //пример
            _invoke(() => HealthText.Content = hp);
        }

        private void GameOver(bool win)
        {
            string result = win ? "You win" : "You loose";
            _invoke(() =>
            {
                new MessageBox(result) { Owner = Parent as Window }.ShowDialog();
                _exit();
            });



        }


        private void _invoke(Action action)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(action);
            else
                action();
        }
        private Game()
        {
            InitializeComponent();
            Cursor = new Cursor(System.IO.Path.GetFullPath(@"../Data/Pictures/curOfBayraktar.cur"));
            Focus();

            //_clockTimer = new Timer(1000);
            //_clockTimer.Elapsed += clockTimer_Elapsed;
            //_clockTimer.Start();
            //DataContext = this;

            //Pause.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\pause.png") as ImageSource;

            //Canvas.SetTop(HealthText, SystemParameters.PrimaryScreenHeight - 50);
            //Canvas.SetTop(RandomText1, SystemParameters.PrimaryScreenHeight - 50);
        }


        List<AnimationClock> clocks = new List<AnimationClock>();
        private void AddMilitaryUnit()
        {
            //DoubleAnimation DA = new DoubleAnimation
            //{
            //    From = -300,
            //    To = SystemParameters.PrimaryScreenHeight + 250,
            //};

            ////if (MinSpeed >= 10)
            ////    MinSpeed -= (short)rnd.Next(1, 3);
            ////if (MaxSpeed >= 5)
            ////    MaxSpeed -= (short)rnd.Next(1, 3);

            ////if (MinSpeed > MaxSpeed)
            ////{
            ////    (MinSpeed, MaxSpeed) = (MaxSpeed, MinSpeed);
            ////}

            ////short seconds = (short)rnd.Next(MinSpeed, MaxSpeed);

            ////  DA.Duration = TimeSpan.FromSeconds(seconds);
            //short left = (short)rnd.Next(-50, 50);

            ////todo

            //MilitaryUnit militaryU = new MilitaryUnit(_newUnit());
            //Canvas.SetLeft(militaryU, left);
            //Canvas.SetTop(militaryU, -300);
            //militaryU.Width = Road.Width / 3;
            //militaryU.Height = Road.Width / 3;
            //AnimationClock clock;
            //DA.Completed += (s, e) =>
            //{
            //    //HealthPoints--;
            //    //if (HealthPoints == 0)
            //    //{
            //    //    PauseFunc("Поразка", true);
            //    //}
            //    //if (HealthPoints <= 5)
            //    //{
            //    //    HealthText.Content = "Health: " + HealthPoints.ToString();
            //    //}

            //};
            //clock = DA.CreateClock();
            //clocks.Add(clock);
            //militaryU.MouseLeftButtonUp += MilitaryUnit_MouseLeftButtonUp;
            //militaryU.ApplyAnimationClock(Canvas.TopProperty, clock);
            //int temp = rnd.Next(0, CMUs.Children.Count);
            //while (!(CMUs.Children[temp] is Border))
            //{
            //    temp = rnd.Next(0, 3);
            //}
            //var this_ = (CMUs.Children[temp] as Border).Child;
            //(this_ as Canvas).Children.Add(militaryU);

        }

        private Unit _newUnit()
        {
            return null;
        }


        Random rnd = new Random();
        DispatcherTimer dispatcherTimer;
        //private Timer _clockTimer;




        private async void MilitaryUnit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is MilitaryUnit)
            {
                var MU = (sender as MilitaryUnit);
                MU.BeginAnimation(Canvas.TopProperty, new DoubleAnimation() { To = Canvas.GetTop(MU) });
                //Score += rnd.Next(50, 200);

                MU.MouseLeftButtonUp -= MilitaryUnit_MouseLeftButtonUp;

                MU.IsHitTestVisible = false;

                //HealthPoints++;

                //lblScore.Content = "Score: " + Score.ToString();

                //await Task.Delay(TimeSpan.FromSeconds(3));
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
            //PauseAnimation();
            PauseWindow pauseWindow = new PauseWindow(title, isDefeat);
            pauseWindow.ShowDialog();
            if (pauseWindow.DialogResult.HasValue && pauseWindow.DialogResult.Value)
            {
                _exit();
            }
            else
            {
                ResumeAnimation();
                Rect.Visibility = Visibility.Hidden;
                //dispatcherTimer.Start();
            }

        }

        private void _exit()
        {
            (Parent as Window).Content = new MainMenu();
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
            //SaveResult saveResult = new SaveResult();
            //saveResult.ShowDialog();
            //this.User = new User() { Name = Environment.UserName, TimeOf_AtteptEnd = DateTime.Now, Score = this.Score };
            //if (!string.IsNullOrWhiteSpace(saveResult.Name))
            //    User.Name = saveResult.Name;
        }

        private void Game_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            _client.Shoot(p.X, p.Y);
           // _client.SetUnit(_client.Units[0], 0, 0);
        }
    }
}