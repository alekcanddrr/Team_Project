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
        private DispatcherTimer GameTime = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private DispatcherTimer TimeOut = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private DateTime StartTime;
        private TimeSpan TimerGame = TimeSpan.FromMinutes(10);
        private DateTime StartTimeOut = DateTime.Now;
        private TimeSpan TimerTimeOut = TimeSpan.FromMinutes(1);
        private DateTime PauseTime;
        private int Quarter = 1;
        private bool play;

        public NewGameWindow(GameType gameType, Team team1, Team team2)
        {
            InitializeComponent();
            GameTime.Tick += TimeTick;
            TimeOut.Tick += TimeOutTimer_Tick;

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
            TimeSpan TimeLeft = TimerGame - (DateTime.Now - StartTime);
            if (TimeLeft <= TimeSpan.Zero)
                TimerStop();
            else
                txtTime.Text = TimeLeft.Minutes + ":" + TimeLeft.Seconds;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = false;
            btnStart.IsEnabled = true;
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
                TimerGame = TimeSpan.FromMinutes(10);
                txtTime.Text = TimerGame.Minutes + ":" + TimerGame.Seconds;
            }
            else
                GameOver();
        }

        private void TimerPause()
        {
            GameTime.Stop();
            PauseTime = DateTime.Now;
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

        private void TimeOutTimer()
        {
            TimerPause();

            StartTimeOut = DateTime.Now;
            TimerTimeOut = TimeSpan.FromMinutes(1);

         //   btnStart.IsEnabled = false;
         //   btnStop.IsEnabled = false;

            TimeOut.Start();

        }

        private void TimeOutTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan TimeLeft = TimerTimeOut - (DateTime.Now - StartTimeOut);
            if (TimeLeft <= TimeSpan.Zero)
            {
                TimeOut.Stop();
                btnStart.IsEnabled = true;
                TimeLeft = TimerGame - (DateTime.Now - StartTime);
                txtTime.Text = TimeLeft.Minutes + ":" + TimeLeft.Seconds;
            }
            else
                txtTime.Text = TimeLeft.Minutes + ":" + TimeLeft.Seconds;
        }
    }
}
