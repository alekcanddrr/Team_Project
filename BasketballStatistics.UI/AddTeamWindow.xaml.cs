using System;
using System.ComponentModel;
using System.Windows;
// Project classes.
using BasketballStatistics.Data;

namespace BasketballStatistics.UI
{
    /// <summary>
    /// Логика взаимодействия для AddNewItemWindow.xaml
    /// </summary>
    public partial class AddTeamWindow : Window
    {
        Repository _repository;

        public AddTeamWindow()
        {
            InitializeComponent();
            _repository = new Repository();

            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Trying to add a new team to the DB.
            try
            {
                var teamName = txtName.Text;
                var coachName = txtCoachName.Text;
                var coachSurname = txtCoachSurName.Text;
                _repository.AddTeamInDatabase(teamName, coachName, coachSurname);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}