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
        Window _window => Parent as Window;
        public User User { get; set; }

        public Game(GameClient client, GameRole gameRole) : this()
        {
            Focus();
            _client = client;
            _client.HealthChanged += HealthChanged;
            _client.ScoreChanged += ScoreChanged;
            _client.DestroyUnit+=DestroyUnit;
            _client.GameOver += GameOver;
            _client.SetUnitAction += SetUnitAction;
            _gameRole = gameRole;
            switch (gameRole)
            {
                case GameRole.Attack:
                    //todo
                    MouseUp += Game_OnMouseUp;
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

        private void DestroyUnit(int tag)
        {
            foreach (UserControl control in cnv.Children)
            {
                if(!(control is MilitaryUnit unit))
                    continue;
                if ((int)unit.Tag == tag)
                {
                    unit.Destroy();
                }
            }
        }

        private void GameOver(GameClient obj)
        {
            string result = "You loose";
            _invoke(() =>
            {
                new MessageBox(result) { Owner = Parent as Window }.ShowDialog();
                _exit();
            });
        }

        private void SetUnitAction(MessageUnit unitData)
        {
            _invoke(() => _setUnit(unitData));
        }

        private void _setUnit(MessageUnit unitData)
        {
            MilitaryUnit unit = new MilitaryUnit(unitData.Unit)
            {
                Tag = unitData.Id
            };
            unit.IsDestroyed+= IsDestroyed;
            if (_gameRole == GameRole.Defense)
            {
                unit.MouseLeftButtonUp += UnitDestroy_MouseUp;
            }
            DoubleAnimation DA = new DoubleAnimation
            {
                From = -unit.Y,
                To = SystemParameters.PrimaryScreenHeight + 250,
                Duration = TimeSpan.FromSeconds(unitData.Speed)
            };
            Canvas.SetLeft(unit, unitData.Coords.X);
            Canvas.SetTop(unit, -unit.Y);
            DA.Completed += (s, e) =>
            {
                _hit(unit);
            };
            var clock = DA.CreateClock();
            clocks.Add(clock);
            //unit.MouseLeftButtonUp += MilitaryUnit_MouseLeftButtonUp;
            unit.ApplyAnimationClock(Canvas.TopProperty, clock);
            cnv.Children.Add(unit);
        }

        private void IsDestroyed(MilitaryUnit unit)
        {
            unit.BeginAnimation(Canvas.TopProperty, new DoubleAnimation() { To = Canvas.GetTop(unit) }); 
            unit.IsHitTestVisible = false;
        }

        private void UnitDestroy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is MilitaryUnit unit)
            {
                unit.Destroy();
                _client.UnitDestroy((int)unit.Tag);
            }
        }

        private void _hit(MilitaryUnit unit)
        {
            _client.Shoot();
            cnv.Children.Remove(unit);
        }

        private void ScoreChanged(int score)
        {
            //пример
            //todo
            _invoke(() => lblScore.Content = score);
        }
        private void HealthChanged(int hp)
        {
            //пример
            //todo
            _invoke(() => HealthText.Content = hp);
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


        Random rnd = new Random();
        DispatcherTimer dispatcherTimer;
        //private Timer _clockTimer;





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
            // Rect.Visibility = Visibility.Visible;
            PauseAnimation();
            PauseWindow pauseWindow = new PauseWindow(title, isDefeat);
            pauseWindow.ShowDialog();
            if (pauseWindow.DialogResult.HasValue && pauseWindow.DialogResult.Value)
            {
                _client.Exit();
            }
            else
            {
                ResumeAnimation();
                //   Rect.Visibility = Visibility.Hidden;
                //dispatcherTimer.Start();
            }

        }

        private void _exit()
        {

            try
            {
                _window.Content = new MainMenu();

            }
            catch (Exception e)
            {

                new MessageBox("Something went wrong").ShowDialog();
                Application.Current.Shutdown();
            }
        }

        private void _pause()
        {
            PauseFunc("На паузі", false);
        }

        private void Pause_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && e.IsRepeat == false)
                _pause();
        }

        private void Game_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            _client.SetUnit((int)p.X);
        }
    }
}