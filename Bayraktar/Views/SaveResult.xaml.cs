using System.Windows;

namespace Bayraktar
{
    /// <summary>
    /// Логика взаимодействия для SaveResult.xaml
    /// </summary>
    public partial class SaveResult : Window
    {
        public string Name { get; set; }
        public SaveResult()
        {
            InitializeComponent();

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Name = NameTB.Text;
            Close();
        }
    }
}
