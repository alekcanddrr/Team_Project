using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
// Our library.
using BasketballStatistics.Data;

namespace BasketballStatistics.UI
{
    /// <summary>
    /// In this window we choose 2 teams (loaded from the DB) to play. Then pass it to the next window.
    /// </summary>
    public partial class AddGameWindow : Window
    {
        Repository _repository = new Repository();

        public AddGameWindow()
        {
            InitializeComponent();
            // Adding teams from the DB into ComboBoxes.
            AddTeams();

            firstTeamBox.SelectionChanged += firstTeamBox_Selected;
            secondTeamBox.SelectionChanged += secondTeamBox_Selected;
            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();
        }

        private void AddTeams()
        {
            // Parallel adding teams into ComboBoxes. So the program doesn't stop.
            Task task = new Task(() => Dispatcher.Invoke(() =>
                {
                    foreach (var team in _repository.AllTeams)
                    {
                        firstTeamBox.Items.Add(new ComboBoxItem { Content = team.Name });
                        secondTeamBox.Items.Add(new ComboBoxItem { Content = team.Name });
                    }
                    // Making selected the first item. The second will be selected in the second ComboBox, so it is not enabled here.
                    firstTeamBox.SelectedIndex = 0;
                    (firstTeamBox.Items[1] as ComboBoxItem).IsEnabled = false;
                    // Similarly in the second ComboBox.
                    secondTeamBox.SelectedIndex = 1;
                    (secondTeamBox.Items[0] as ComboBoxItem).IsEnabled = false;
                }));
            // When the task is completed - making hidden our label with "Loading" text.
            task.ContinueWith(t => Dispatcher.Invoke(() => loadingLabel.Visibility = Visibility.Hidden));
            task.Start();
        }

        private void firstTeamBox_Selected(object sender, RoutedEventArgs e)
        {
            // Making all enabled (in the second).
            foreach (var comboBoxItem in secondTeamBox.Items)
                (comboBoxItem as ComboBoxItem).IsEnabled = true;
            // Making disabled an item in the second, which was selected.
            (secondTeamBox.Items[firstTeamBox.SelectedIndex] as ComboBoxItem).IsEnabled = false;
        }

        private void secondTeamBox_Selected(object sender, RoutedEventArgs e)
        {
            // Making all enabled (in the first).
            foreach (var comboBoxItem in firstTeamBox.Items)
                (comboBoxItem as ComboBoxItem).IsEnabled = true;
            // Making disabled an item in the first, which was selected.
            (firstTeamBox.Items[secondTeamBox.SelectedIndex] as ComboBoxItem).IsEnabled = false;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Getting teams by names.
                var team1 = _repository.FindTeam(firstTeamBox.SelectionBoxItem.ToString());
                var team2 = _repository.FindTeam(secondTeamBox.SelectionBoxItem.ToString());
                // Passing these teams to the new window.
                NewGameWindow gameWindow = new NewGameWindow(GameType.New, team1, team2, txtPlace.Text) { Owner = Owner };
                gameWindow.Show();
                // Closing this window.
                Close();
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