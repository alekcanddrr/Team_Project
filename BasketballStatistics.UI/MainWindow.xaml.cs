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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasketballStatistics.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddGameWindow addWindow = new AddGameWindow { Owner = this };
            addWindow.Show();
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            AllGamesWindow gamesWindow = new AllGamesWindow { Owner = this };
            gamesWindow.Show();
        }

        private void btnNewTeam_Click(object sender, RoutedEventArgs e)
        {
            AddTeamWindow itemWindow = new AddTeamWindow() { Owner = this };
            itemWindow.Show();
        }
    }
}