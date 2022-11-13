using BayraktarGame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bayraktar {
    /// <summary>
    /// Interaction logic for AttackerWin.xaml
    /// </summary>
    public partial class AttackerWin : Window 
    {

        ObservableCollection<Unit> MilitaryUnits;
        public AttackerWin() 
        {
            InitializeComponent();

            ObservableCollection<Unit> MilitaryUnits = new ObservableCollection<Unit>();
            MilitaryUnits.Add(new Unit() { Id = 0, CoolDown = 500, Name = "ArtZ", Price = 100 });
            MilitaryUnits.Add(new Unit() { Id = 1, CoolDown = 400, Name = "BTR", Price = 100 });

            MilitaryUnitsLists.ItemsSource = MilitaryUnits; //подвязать существующих юнитов в список

            //MilitaryUnitsLists.Width = 550;
            //for (int i = 0; i < 250; i++) {
            //    MilitaryUnitsLists.Items.Add(i);
            //}
        }

        private void AttackerMenu_MouseEnter(object sender, MouseEventArgs e) 
        {
            AttackerMenuCanvas.Margin = new Thickness(40,0,0,0);
        }

        private void AttackerMenu_MouseLeave(object sender, MouseEventArgs e) 
        {
            AttackerMenuCanvas.Margin = new Thickness(590, 0, 0, 0);
        }
    }
}
