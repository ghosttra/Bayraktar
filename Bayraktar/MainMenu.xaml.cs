using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Bayraktar
{
    public partial class MainMenu : Window
    {
        private DispatcherTimer cloudLeftTimer = new DispatcherTimer();
        private DispatcherTimer cloudRightTimer = new DispatcherTimer();

        private void SetTimer(DispatcherTimer timer, int seconds)
        {
            timer.Stop();
            timer.Interval = TimeSpan.FromSeconds(seconds);
            timer.Tick -= CloudTimer_Tick;
            timer.Tick += CloudTimer_Tick;
            timer.Start();
        }
        public MainMenu()
        {
            InitializeComponent();

            load_method();

            if (users.Count > 0)
                Rating.IsEnabled = true;

            try
            {
                CloudLeft.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\cloud.png") as ImageSource;
                CloudRight.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\cloud.png") as ImageSource;
                Bayraktar.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\Bayraktar.png") as ImageSource;
                Logo.Source = new ImageSourceConverter().ConvertFromString(@"..\Data\Pictures\logo.png") as ImageSource;
            }
            catch (Exception)
            {
            }


            SetCloudAnimation(cloudLeftTimer, CloudLeft, true);
            SetCloudAnimation(cloudRightTimer, CloudRight, true);
        }

        Random rnd = new Random();

        private void SetCloudAnimation(DispatcherTimer timer, Image cloud, bool access)
        {
            if (access || rnd.Next(0, 100) % 2 == 0)
            {
                DoubleAnimation DA = new DoubleAnimation
                {
                    From = -200,
                    To = System.Windows.SystemParameters.PrimaryScreenHeight + 100,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                short seconds = (short)rnd.Next(5, 15);
                DA.Duration = TimeSpan.FromSeconds(seconds);

                SetTimer(timer, seconds);

                cloud.BeginAnimation(Canvas.TopProperty, DA);

                RotateTransform rotate = new RotateTransform(rnd.Next(1, 3) == 2 ? 270 : 90);
                cloud.RenderTransform = rotate;
            }
        }

        private void CloudTimer_Tick(object sender, EventArgs e)
        {
            if (sender != null && sender is DispatcherTimer)
            {
                DispatcherTimer tempTimer = sender as DispatcherTimer;

                if (tempTimer == cloudLeftTimer)
                {
                    if (rnd.Next(0, 100) % 2 == 0)
                        CloudLeft.Visibility = CloudLeft.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

                    if (CloudRight.Visibility != Visibility.Hidden)
                    {
                        Canvas.SetLeft(CloudLeft, rnd.Next(-100, 100));
                        SetCloudAnimation(tempTimer, CloudLeft, false);
                    }
                }
                else
                {
                    if (rnd.Next(0, 100) % 5 == 0)
                        CloudRight.Visibility = CloudRight.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
                    if (CloudRight.Visibility != Visibility.Hidden)
                    {
                        Canvas.SetLeft(CloudRight, rnd.Next(-100, 100));
                        SetCloudAnimation(tempTimer, CloudRight, false);
                    }
                }
            }

        }



        private void removeClouds()
        {
            cloudLeftTimer.Stop();
            cloudRightTimer.Stop();

            CloudLeft.BeginAnimation(TopProperty, new DoubleAnimation());
            CloudRight.BeginAnimation(TopProperty, new DoubleAnimation());
        }

        private void setClouds()
        {
            Canvas.SetTop(CloudLeft, -200);
            Canvas.SetTop(CloudRight, -200);
            SetCloudAnimation(cloudLeftTimer, CloudLeft, true);
            SetCloudAnimation(cloudRightTimer, CloudRight, true);
        }

        List<User> users = new List<User>();

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                removeClouds();
                switch (button.Name)
                {
                    case "Rating":
                        RatingWindow ratingWindow = new RatingWindow(users);
                        ratingWindow.ShowDialog();
                        break;
                    case "Start":
                        Game game = new Game();
                        game.ShowDialog();
                        if (game.User.Score != 0)
                            users.Add(game.User);
                        if (users.Count == 1)
                            Rating.IsEnabled = true;
                        break;
                    default:
                        Close();
                        break;
                }
                setClouds();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            save_method();
        }
        private void save_method()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));

            try
            {
                using (FileStream fs = new FileStream(@"..\Data\PersonalData\Results.xml", FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, users);
                }
            }
            catch (Exception)
            {
            }

        }
        private void load_method()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));

            try
            {
                using (FileStream fs = new FileStream(@"..\Data\PersonalData\Results.xml", FileMode.OpenOrCreate))
                {
                    users = xmlSerializer.Deserialize(fs) as List<User>;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
