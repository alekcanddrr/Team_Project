using System;
using System.Collections.Generic;
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
// Data library.
using BasketballStatistics.Data;

namespace BasketballStatistics.UI
{
    public enum GameType
    {
        New, Old
    }
    /// <summary>
    /// Логика взаимодействия для NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        public NewGameWindow(GameType gameType, Team team1, Team team2)
        {
            InitializeComponent();

            MessageBox.Show(team1.Name + " " + team2.Name);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
