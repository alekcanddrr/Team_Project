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
            int age;
            double height, weight;

            try
            {
                // Checking if input values are valid.
                if (!int.TryParse(txtAge.Text, out age))
                    throw new ArgumentException("Incorrect format of age! It must be a positive, integer value.");
                if (!double.TryParse(txtHeight.Text, out height))
                    throw new ArgumentException("Inccorect format of height! It must be a positive, numeric value.");
                if (!double.TryParse(txtWeight.Text, out weight))
                    throw new ArgumentException("Incorrect format of weight! It must be a positive, numeric value.");

                var team = comboBoxTeam.SelectedItem as Team;
                if (team == null)
                    throw new ArgumentException("Something went wrong.");
                var position = comboBoxPosition.SelectionBoxItem.ToString();
                
                // Adding to the DB.
                //_repository.AddPlayerInDatabase(txtName.Text, txtSurname.Text, height, weight, age, position, team);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}