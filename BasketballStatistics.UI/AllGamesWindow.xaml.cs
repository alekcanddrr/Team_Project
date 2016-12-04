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
        public AllGamesWindow()
        {
            InitializeComponent();
            // To make focus on the owner window (MainWindow).
            Closing += (object sender, CancelEventArgs e) => Owner.Focus();

            for (int i = 0, j = 10, k = 20; i < 10 || j > 0 || k > 0; i++, j--, k -= 2)
                teamsList.Items.Add(new { Name = "Команда №" + (i+1), Wins = i, Draws = j, Loses = k });

            for (int i = 0, j = 10; i < 10 || j > 0; i++, j--)
                gamesList.Items.Add(new { FirstTeam = "Команда №" + (i+1), Score = i+":"+j, SecondTeam = "Команда №"+j });
        }

        private void btnGameInfo_Click(object sender, RoutedEventArgs e)
        {
            GameResultWindow resultWindow = new GameResultWindow { Owner = this };
            resultWindow.Show();
        }

        private void gamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnGameInfo.IsEnabled = true;
        }
    }
}
