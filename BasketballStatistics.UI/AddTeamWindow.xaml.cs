﻿using System;
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
// Project classes.
using BasketballStatistics.Data;

namespace BasketballStatistics.UI
{
    /// <summary>
    /// Логика взаимодействия для AddNewItemWindow.xaml
    /// </summary>
    public partial class AddTeamWindow : Window
    {
        public AddTeamWindow()
        {
            InitializeComponent();
            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Team team = new Team { Name = txtName.Name };
            Close();
            Owner.Focus();
        }
    }
}