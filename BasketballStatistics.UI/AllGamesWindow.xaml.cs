using BasketballStatistics.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BasketballStatistics.UI
{
    /// <summary>
    /// Логика взаимодействия для AllGamesWindow.xaml
    /// </summary>
    public partial class AllGamesWindow : Window
    {
        Repository _repository = new Repository();

        public AllGamesWindow()
        {
            InitializeComponent();
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
            //NewGameWindow resultWindow = new NewGameWindow(GameType.Old) { Owner = this };
            //resultWindow.Show();
        }

        private void gamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnGameInfo.IsEnabled = true;
        }

        private void gamesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //NewGameWindow resultWindow = new NewGameWindow(GameType.Old) { Owner = this };
            //resultWindow.Show();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}