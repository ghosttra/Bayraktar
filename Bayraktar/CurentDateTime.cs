using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace Bayraktar
{
    partial class Game
    {
      

        
        void clockTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(UpdateCurrentDateTime));
        }



        public string CurrentDateTime
        {
            get { return (string)GetValue(CurrentDateTimeProperty); }
            set { SetValue(CurrentDateTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentDateTimeProperty =
            DependencyProperty.Register("CurrentDateTime", typeof(string), typeof(Game), new UIPropertyMetadata(string.Empty));

        private void UpdateCurrentDateTime()
        {
            CurrentDateTime = DateTime.Now.ToString("MM/dd/yyyy      HH:mm:ss");
        }
    }

}
