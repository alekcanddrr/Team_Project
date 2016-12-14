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
        DateTime? SelectedDate;
        Repository _repository = new Repository();
        public AddPlayerWindow()
        {
            InitializeComponent();
            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();

            LoadTeams();
        }

        private void LoadTeams()
        {
            // Parallel loading all teams.
            Task t = new Task(() => Dispatcher.Invoke(() =>
            {
                comboBoxTeam.ItemsSource = _repository.AllTeams;
                comboBoxTeam.SelectedIndex = 0;
            }));
            t.Start();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name, surname;
            double height, weight;

            try
            {
                // Checking if input values are valid.
                name = txtName.Text;
                surname = txtSurname.Text;
                if (String.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null or whitespaces");
                if (String.IsNullOrWhiteSpace(surname))
                    throw new ArgumentException("Surname cannot be null or whitespaces");
                if (!double.TryParse(txtHeight.Text, out height) || height <= 0)
                    throw new ArgumentException("Inccorect format of height! It must be a positive, numeric value.");
                if (!double.TryParse(txtWeight.Text, out weight) || weight <= 0)
                    throw new ArgumentException("Incorrect format of weight! It must be a positive, numeric value.");

                var team = comboBoxTeam.SelectedItem as Team;
                if (team == null)
                    throw new ArgumentException("Something went wrong.");
                var position = comboBoxPosition.SelectionBoxItem.ToString();
                
                // Adding to the DB.
                _repository.AddPlayerInDatabase(txtName.Text, txtSurname.Text, height, weight, (DateTime)SelectedDate, position, team);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void BirthDatePickers_date_changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectedDate = BirthDatePicker.SelectedDate;
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}