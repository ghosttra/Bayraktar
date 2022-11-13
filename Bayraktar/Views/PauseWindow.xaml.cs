using System.Windows;

namespace Bayraktar
{
    /// <summary>
    /// Логика взаимодействия для PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        public PauseWindow(string title, bool isDefeat)
        {
            InitializeComponent();
            TitleBlock.Text = title;
            ContinueBtn.IsEnabled = !isDefeat;
            Height /= 2;
            Width /= 2;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
