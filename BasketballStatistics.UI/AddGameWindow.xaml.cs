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
    /// Логика взаимодействия для AddGameWindow.xaml
    /// </summary>
    public partial class AddGameWindow : Window
    {
        public AddGameWindow()
        {
            InitializeComponent();

            firstTeamBox.SelectionChanged += firstTeamBox_Selected;
            secondTeamBox.SelectionChanged += secondTeamBox_Selected;
            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            NewGameWindow gameWindow = new NewGameWindow(GameType.New) { Owner = this };
            gameWindow.Show();
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
    }
}