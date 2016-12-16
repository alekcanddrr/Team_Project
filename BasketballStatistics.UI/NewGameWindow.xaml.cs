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
using System.Timers;
using System.Windows.Threading;
// Data library.
using BasketballStatistics.Data;

namespace BasketballStatistics.UI
{
    public enum GameType
    {
        New, Old
    }
    /// <summary>
    /// Логика взаимодействия для NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        private DispatcherTimer GameTime = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10)};
        private DateTime StartTime;
        private TimeSpan Timer = TimeSpan.FromMinutes(1);
        private DateTime PauseTime;
        private int Quarter = 1;
        private bool play;

        public NewGameWindow(GameType gameType, Team team1, Team team2)
        {
            InitializeComponent();
            GameTime.Tick += TimeTick;

            MessageBox.Show(team1.Name + " " + team2.Name);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            GameOver();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            
            if (!play)
                TimerStart();
            else
                TimerRelease();

            btnStop.IsEnabled = true;
            btnStart.IsEnabled = false;
        }

        private void TimeTick(object sender, EventArgs e)
        {
            TimeSpan TimeLeft = Timer - (DateTime.Now - StartTime);
            if (TimeLeft <= TimeSpan.Zero)
                TimerStop();
            else
            txtTime.Text = TimeLeft.Minutes + ":" + TimeLeft.Seconds;
        }
 
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = false;
            TimerPause();
        }

        private void TimerRelease()
        {
            StartTime = StartTime.Add(DateTime.Now - PauseTime);
            GameTime.Start();
        }

        private void TimerStop()
        {
            play = false;
            if (Quarter < 4)
            {
                GameTime.Stop();
                btnStart.IsEnabled = true;
                Quarter++;
                txtQuarter.Text = Quarter.ToString();
                Timer = TimeSpan.FromMinutes(10);
                txtTime.Text = Timer.Minutes + ":" + Timer.Seconds;
            }
            else
                GameOver();
        }

        private void TimerPause()
        {
            GameTime.Stop();
            PauseTime = DateTime.Now;
            btnStart.IsEnabled = true;
        }

        private void TimerStart()
        {
            play = true;
            StartTime = DateTime.Now;
            txtQuarter.Text = Quarter.ToString();
            GameTime.Start();
        }

        private void GameOver()
        {
            MessageBox.Show("Game over!");
            Close();
        }
    }
}
