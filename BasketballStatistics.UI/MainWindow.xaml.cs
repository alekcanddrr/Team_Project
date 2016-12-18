using System.Windows;

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
            AddTeamWindow addTeamWindow = new AddTeamWindow { Owner = this };
            addTeamWindow.Show();
        }

        private void btnNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            AddPlayerWindow addPlayerWindow = new AddPlayerWindow { Owner = this };
            addPlayerWindow.Show();
        }

        private void btnAllPlayers_Click(object sender, RoutedEventArgs e)
        {
            AllPlayersWindow allPlayersWindow = new AllPlayersWindow { Owner = this };
            allPlayersWindow.Show();
        }
    }
}