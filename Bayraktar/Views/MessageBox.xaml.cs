using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;

namespace Bayraktar
{

    public partial class MessageBox : Window
    {
        private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
        public DialogResult Result;

        public MessageBox(string info, List<Button> buttons)
        {
            _init();
            InfoLabel.Text = info;
            foreach (var button in buttons)
            {
                ButtonPanel.Children.Add(button);
            }
        }
        public MessageBox(string unitName, bool lines) {
            InitializeComponent();
            _init();
            InfoLabel.Text = unitName;
            List<Button> buttonsList = new List<Button>();
            buttonsList.Add(_buttons["line1"]);
            buttonsList.Add(_buttons["line2"]);
            buttonsList.Add(_buttons["line3"]);
            foreach (var button in buttonsList) {
                ButtonPanel.Children.Add(button);
            }
        }
        public MessageBox(string info, MessageBoxButton buttons)
        {
            _init();
            InfoLabel.Text = info;
            _setBtn(buttons);
        }

        private void _setBtn(MessageBoxButton buttons)
        {
            List<Button> buttonsList = new List<Button>();
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    buttonsList.Add(_buttons["Ok"]);
                    break;
                case MessageBoxButton.OKCancel:

                    buttonsList.Add(_buttons["Ok"]);
                    buttonsList.Add(_buttons["Cancel"]);
                    break;

                case MessageBoxButton.YesNo:
                    buttonsList.Add(_buttons["Yes"]);
                    buttonsList.Add(_buttons["No"]);
                    break;
                case MessageBoxButton.YesNoCancel:
                    buttonsList.Add(_buttons["Yes"]);
                    buttonsList.Add(_buttons["No"]);
                    buttonsList.Add(_buttons["Cancel"]);
                    break;
            }
            foreach (var button in buttonsList)
            {
                ButtonPanel.Children.Add(button);
            }

        }

        private void _init()
        {
            InitializeComponent();

            var ok = new Button
            {
                Tag = "Ok", Content = "Ok",
                IsDefault = true
            };
            ok.Click += Ok_Click;
            _buttons.Add(ok.Tag.ToString(), ok);
            var cancel = new Button
            {
                Tag = "Cancel",
                Content = "Cancel",
                IsCancel = true
            };
            cancel.Click += Cancel_Click; 
            _buttons.Add(cancel.Tag.ToString(), cancel);
            var yes = new Button
            {
                Tag = "Yes",
                Content = "Yes",
                IsDefault = true
            };
            yes.Click += Yes_Click; 
            _buttons.Add(yes.Tag.ToString(), yes);
            var no = new Button
            {
                Tag = "No",
                Content = "No", IsCancel = true
            };
            no.Click += No_Click; 
            _buttons.Add(no.Tag.ToString(), no);
            var line1 = new Button {
                Tag = "line1",
                Content = "First Line",
                IsDefault = true
            };
            line1.Click += Lines_Click;
            _buttons.Add(line1.Tag.ToString(), line1);
            var line2 = new Button {
                Tag = "line2",
                Content = "Second Line",
                IsDefault = true
            };
            line2.Click += Lines_Click;
            _buttons.Add(line2.Tag.ToString(), line2);
            var line3 = new Button {
                Tag = "line3",
                Content = "Third Line",
                IsDefault = true
            };
            line3.Click += Lines_Click;
            _buttons.Add(line3.Tag.ToString(), line3);
        }
        public short unitLine { get; set; }
        private void Lines_Click(object sender, RoutedEventArgs e) {
            var b = sender as Button;
            unitLine = short.Parse(b.Tag.ToString().Substring(4));
            Result = System.Windows.Forms.DialogResult.Yes;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.Cancel;
            DialogResult = false;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.Yes;
            DialogResult = true;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.No;
            DialogResult = false;
        }
        

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.OK;
            DialogResult = true;
        }

        public MessageBox(string info) : this(info, MessageBoxButton.OK)
        {
        }
    }
}
