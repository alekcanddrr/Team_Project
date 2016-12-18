using System;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
// Our library.
using BasketballStatistics.Data;

namespace BasketballStatistics.UI
{
    /// <summary>
    /// Логика взаимодействия для AddPlayerWindow.xaml
    /// </summary>
    public partial class AddPlayerWindow : Window
    {
        Repository _repository;
        public AddPlayerWindow()
        {
            InitializeComponent();
            _repository = new Repository();

            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();

            LoadTeams();
        }

        private async void LoadTeams()
        {
            // Parallel loading all teams.
            await Task.Run(async () => await Dispatcher.Invoke(async () =>
            {
                comboBoxTeam.ItemsSource = await _repository.GetAllTeams();
                comboBoxTeam.SelectedIndex = 0;
            }));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            double height, weight;

            try
            {
                // Checking if input values are valid.
                if (!double.TryParse(txtHeight.Text, out height) || height <= 0)
                    throw new ArgumentException("Inccorect format of height! It must be a positive, numeric value.");
                if (!double.TryParse(txtWeight.Text, out weight) || weight <= 0)
                    throw new ArgumentException("Incorrect format of weight! It must be a positive, numeric value.");
                if (!birthDatePicker.SelectedDate.HasValue)
                    throw new ArgumentException("The date is not mentioned! Please, chose the date of birth.");

                var team = comboBoxTeam.SelectedItem as Team;
                if (team == null)
                    throw new ArgumentException("Something went wrong.");
                var position = comboBoxPosition.SelectionBoxItem.ToString();

                // Adding to the DB.
                _repository.AddPlayerInDatabase(txtName.Text, txtSurname.Text, height, weight, birthDatePicker.SelectedDate.Value, position, team);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}