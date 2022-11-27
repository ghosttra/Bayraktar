using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.FSharp.Core;

namespace Bayraktar
{

    public partial class Game : UserControl
    {

        //Attack or Defense
        private GameRole _gameRole;
        private GameClient _client;
        Window _window => Parent as Window;
        public User User { get; set; }


        private Game()
        {
            InitializeComponent();
            Cursor = new Cursor(System.IO.Path.GetFullPath(@"../Data/Pictures/curOfBayraktar.cur"));
            //Focus();
        }
        public Game(GameClient client, GameRole gameRole, int gameWidth = 1920) : this()
        {
            Focus();
            _client = client;
            _client.HealthChanged += HealthChanged;
            _client.ScoreChanged += ScoreChanged;
            _client.DestroyUnit += DestroyUnit;
            _client.GameOver += GameOver;
            _client.SetUnitAction += SetUnitAction;
            _gameRole = gameRole;
            switch (gameRole)
            {
                case GameRole.Attack:
                    
                    MouseUp += AddUnit;
                    break;
                case GameRole.Defense:
                    break;
            }

        }

        private void DestroyUnit(int tag)
        {
            _invoke(() =>
            {
                foreach (UserControl control in cnv.Children)
                {
                    if (!(control is MilitaryUnit unit))
                        continue;
                    if ((int)unit.Tag == tag)
                    {
                        unit.Destroy();
                    }
                }
            });
        }

        private void GameOver(GameClient client)
        {
            var result = "Game over";
            
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
            MilitaryUnit unit = new MilitaryUnit(unitData.Unit) {
                Tag = unitData.Id
            };
            unit.Destroyed += IsDestroyed;
            if (_gameRole == GameRole.Defense) {
                unit.MouseLeftButtonUp += UnitDestroy_MouseUp;
            }
            DoubleAnimation da = new DoubleAnimation {
                From = -unit.Y,
                To = SystemParameters.PrimaryScreenHeight + unit.Y,
                Duration = TimeSpan.FromSeconds(unitData.Speed)
            };
            var x = (unitData.Coords.X > _window.Width - unit.X) ? (int)(_window.Width - unit.X) : unitData.Coords.X;

            da.Completed += (s, e) => {
                if(!unit.IsDestroying) 
                    _hit(unit);
            };
            unit.Animation = da;
            var clock = da.CreateClock();
            clocks.Add(clock);

            Canvas.SetLeft(unit, x);
            //Canvas.SetTop(unit, -unit.Y);
            unit.ApplyAnimationClock(Canvas.TopProperty, clock);
            //unit.Animation.BeginAnimation(Canvas.TopProperty, da);
            cnv.Children.Add(unit);


        }

        private async void IsDestroyed(MilitaryUnit unit)
        {
            _invoke(() =>
            {
                DoubleAnimation da = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(3000)
                };
                unit.ApplyAnimationClock(Canvas.TopProperty, da.CreateClock());

            });
            await Task.Delay(3000);
            cnv.Children.Remove(unit);
        }

        private void UnitDestroy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is MilitaryUnit unit)
            {
                _client.UnitDestroy((int)unit.Tag);
                unit.MouseLeftButtonUp-=UnitDestroy_MouseUp;
               // unit.ApplyAnimationClock(Canvas.TopProperty, null);
               //unit.Animation.ApplyAnimationClock(Canvas.TopProperty, null);
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


        List<AnimationClock> clocks = new List<AnimationClock>();


        private void PauseAnimation()
        {
            foreach (var clock in clocks)
            {
                clock.Controller?.Pause();
            }
        }
        private void ResumeAnimation()
        {
            foreach (var clock in clocks)
            {
                clock.Controller?.Resume();
            }
        }

        private void PauseFunc(string title, bool isDefeat)
        {
            //Rect.Visibility = Visibility.Visible;
            //PauseAnimation();
            PauseWindow pauseWindow = new PauseWindow(title, isDefeat);
            pauseWindow.ShowDialog();
            if (pauseWindow.DialogResult.HasValue && pauseWindow.DialogResult.Value)
            {
                _client.Exit();
            }
            else
            {
                //ResumeAnimation();
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

        private bool _onCooldown = false;
        private void AddUnit(object sender, MouseButtonEventArgs e)
        {
            if (_onCooldown)
                return;
            var p = e.GetPosition(this);
            _client.SetUnit((int)p.X);
            Cooldown();
        }

        private async void Cooldown()
        {
            _onCooldown = true;
            await Task.Delay(1000);
            _onCooldown = false;
        }
    }
}