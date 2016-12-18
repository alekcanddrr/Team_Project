﻿using System;
using System.Windows;
using System.Windows.Controls;
// Our library.
using BasketballStatistics.Data;

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

            playersList.ItemsSource = _repository.AllPlayers;
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
