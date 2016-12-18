using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
// Our library;
using BasketballStatistics.Data;

namespace BasketballStatistics.UI
{
    /// <summary>
    /// Логика взаимодействия для AllGamesWindow.xaml
    /// </summary>
    public partial class AllGamesWindow : Window
    {
        Repository _repository;

        public AllGamesWindow()
        {
            InitializeComponent();
            _repository = new Repository();

            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();

            FillTables();
        }

        private void FillTables()
        {
            // Parallel loading games and teams from the repository. Then making invisible the labels from the main thread.
            var gameLoadingTask = Task.Run(() => Dispatcher.Invoke(() =>
            {
                gamesList.ItemsSource = _repository.AllMatchesData();
            }));
            gameLoadingTask.ContinueWith(t => Dispatcher.Invoke(() => gamesLoadingLabel.Visibility = Visibility.Hidden));

            var teamLoadingTask = Task.Run(() => Dispatcher.Invoke(() =>
            {
                teamsList.ItemsSource = _repository.ShortStatisticsOfTeams();
            }));
            teamLoadingTask.ContinueWith(t => Dispatcher.Invoke(() => teamsLoadingLabel.Visibility = Visibility.Hidden));
        }

        private void btnGameInfo_Click(object sender, RoutedEventArgs e)
        {
            var match = _repository.FindMatch(gamesList.SelectedItem);
            NewGameWindow resultWindow = new NewGameWindow(GameType.Old, match.Team1, match.Team2, match.Place, match) { Owner = this };
            resultWindow.Show();
        }

        private void gamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnGameInfo.IsEnabled = true;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}