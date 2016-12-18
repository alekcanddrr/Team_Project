using System;
using System.Windows;
using System.Windows.Controls;
// Our library.
using BasketballStatistics.Data;
using System.Threading.Tasks;

namespace BasketballStatistics.UI
{
    /// <summary>
    /// Логика взаимодействия для AllPlayersWindow.xaml
    /// </summary>
    public partial class AllPlayersWindow : Window
    {
        Repository _repository;

        public AllPlayersWindow()
        {
            InitializeComponent();
            _repository = new Repository();

            Loaded += AllPlayersWindow_Loaded;
        }

        private void AllPlayersWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(async () => await Dispatcher.Invoke(async () => playersList.ItemsSource = await _repository.GetAllPlayers()))
                    .ContinueWith(t => Dispatcher.Invoke(() => loadingLabel.Visibility = Visibility.Hidden));
        }

        private void playersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _repository.DeletePlayerFromDatabase(playersList.SelectedItem);
                playersList.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
