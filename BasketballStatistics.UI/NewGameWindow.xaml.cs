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
        // Adding repositories.
        Repository _repository;
        RepositoryGame _gameRepository;

        private DispatcherTimer _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private DispatcherTimer _timeoutTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private DateTime _gameStart, _pauseTime;
        private TimeSpan _gameTime = TimeSpan.FromMinutes(1);
        private DateTime _timeoutStart = DateTime.Now;
        private TimeSpan _timeoutTime = TimeSpan.FromSeconds(10);
        private int _quarter = 1, _firstTeamTimeOuts, _secondTeamTimeOuts;
        private bool _play;

        public NewGameWindow(GameType gameType, Team team1, Team team2, string matchPlace)
        {
            InitializeComponent();
            _gameTimer.Tick += TimeTick;
            _timeoutTimer.Tick += TimeOutTimer_Tick;

            _repository = new Repository();
            _gameRepository = new RepositoryGame(team1, team2, matchPlace);

            txtFirstTeam.Text = team1.Name;
            txtSecondTeam.Text = team2.Name;
            txtFirstTeamCoach.Text = "Coach: " + _repository.FindCoach(team1);
            txtSecondTeamCoach.Text = "Coach: " + _repository.FindCoach(team2);
            txtPlace.Text = "Place: " + matchPlace;
            txtDate.Text =  "Date: " + _gameRepository.Date;
            
            dataGridPlayers.ItemsSource = _gameRepository.Statistics;

            Closing += (object sender, System.ComponentModel.CancelEventArgs e) => Owner.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you want to save data?", "Closing window", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                _gameRepository.GameOver();
                Close();
            }
            else if (result == MessageBoxResult.No)
                Close();
        }

        #region Timer
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!_play)
                TimerStart();
            else
                TimerRelease();

            btnStop.IsEnabled = true;
            btnStart.IsEnabled = false;
        }

        private void TimeTick(object sender, EventArgs e)
        {
            TimeSpan TimeLeft = _gameTime - (DateTime.Now - _gameStart);
            if (TimeLeft <= TimeSpan.Zero)
                TimerStop();
            else
                txtTime.Text = TimeLeft.Minutes + ":" + (TimeLeft.Seconds < 0 ? "0" + TimeLeft.Seconds : TimeLeft.Seconds.ToString());
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = false;
            btnStart.IsEnabled = true;
            TimerPause();
        }

        private void TimerRelease()
        {
            _gameStart = _gameStart.Add(DateTime.Now - _pauseTime);
            _gameTimer.Start();
        }

        private void TimerStop()
        {
            _play = false;
            Sound.TimeIsOver();
            
            if (_quarter < 4)
            {
                _gameTimer.Stop();
                btnStart.IsEnabled = true;
                _quarter++;

                btnFirstTeamTimeOut.IsEnabled = false;
                btnSecondTeamTimeOut.IsEnabled = false;

                if (_quarter==3)
                {
                    _firstTeamTimeOuts = 0;
                    _secondTeamTimeOuts = 0;

                    firstTeamThirdTimeOut.Visibility = Visibility.Visible;
                    secondTeamThirdTimeOut.Visibility = Visibility.Visible;

                    firstTeamFirstTimeOut.Background = Brushes.Green;
                    firstTeamSecondTimeOut.Background = Brushes.Green;
                    secondTeamFirstTimeOut.Background = Brushes.Green;
                    secondTeamSecondTimeOut.Background = Brushes.Green;
                }

                txtQuarter.Text = "Quarter " + _quarter.ToString();
                btnSecondTeamTimeOut.IsEnabled = false;
                btnFirstTeamTimeOut.IsEnabled = false;
                _gameTime = TimeSpan.FromMinutes(10);
                txtTime.Text = _gameTime.Minutes + ":" + (_gameTime.Seconds<0? "0"+_gameTime.Seconds: _gameTime.Seconds.ToString());
            }
            else
                GameOver();
        }

        private void TimerPause()
        {
            _gameTimer.Stop();
            _pauseTime = DateTime.Now;
        }

        private void TimerStart()
        {
            _play = true;

            if (_quarter <= 2 && _firstTeamTimeOuts < 2 || _quarter >= 3 && _firstTeamTimeOuts < 3)
                btnFirstTeamTimeOut.IsEnabled = true;
            if (_quarter <= 2 && _secondTeamTimeOuts < 2 || _quarter >= 3 && _secondTeamTimeOuts < 3)
                btnSecondTeamTimeOut.IsEnabled = true;

            _gameStart = DateTime.Now;
            txtQuarter.Text = "Quarter " + _quarter.ToString();
            _gameTimer.Start();
        }

        private void GameOver()
        {
            MessageBox.Show("Game over!");
            Close();
        }

        private void TimeOutTimer(string TeamTimeOut)
        {
            if (_gameTimer.IsEnabled)
                TimerPause();

            txtQuarter.Text = TeamTimeOut;
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = false;
            btnFirstTeamTimeOut.IsEnabled = false;
            btnSecondTeamTimeOut.IsEnabled = false;

            _timeoutStart = DateTime.Now;
            _timeoutTimer.Start();

        }

        private void TimeOutTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan TimeLeft = _timeoutTime - (DateTime.Now - _timeoutStart);
            if (TimeLeft <= TimeSpan.Zero)
            {
                Sound.TimeIsOver();

                _timeoutTimer.Stop();
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = false;

                if (_quarter <= 2 && _firstTeamTimeOuts < 2 || _quarter >= 3 && _firstTeamTimeOuts < 3)
                    btnFirstTeamTimeOut.IsEnabled = true;
                if (_quarter <= 2 && _secondTeamTimeOuts < 2 || _quarter >= 3 && _secondTeamTimeOuts < 3)
                    btnSecondTeamTimeOut.IsEnabled = true;

                txtQuarter.Text = "Quarter " + _quarter.ToString();
                TimeLeft = _gameTime - (DateTime.Now - _gameStart.Add(DateTime.Now - _pauseTime));
                txtTime.Text = TimeLeft.Minutes + ":" + (TimeLeft.Seconds < 0 ? "0" + TimeLeft.Seconds : TimeLeft.Seconds.ToString());
            }
            else
                txtTime.Text = TimeLeft.Minutes + ":" + (TimeLeft.Seconds < 0 ? "0" + TimeLeft.Seconds : TimeLeft.Seconds.ToString());
        }

        private void btnFirstTeamTimeOut_Click(object sender, RoutedEventArgs e)
        {
            string s = "Time Out by 1 team";
            _firstTeamTimeOuts++;

            if (_firstTeamTimeOuts == 1)
                firstTeamFirstTimeOut.Background = Brushes.Red;
            else if (_firstTeamTimeOuts == 2)
                firstTeamSecondTimeOut.Background = Brushes.Red;
            else if (_firstTeamTimeOuts == 3)
                firstTeamThirdTimeOut.Background = Brushes.Red;

            TimeOutTimer(s);
        }

        private void btnSecondTeamTimeOut_Click(object sender, RoutedEventArgs e)
        {
            string s = "Time Out by 2 team";
            _secondTeamTimeOuts++;

            if (_secondTeamTimeOuts == 1)
                secondTeamFirstTimeOut.Background = Brushes.Red;
            else if (_secondTeamTimeOuts == 2)
                secondTeamSecondTimeOut.Background = Brushes.Red;
            else if (_secondTeamTimeOuts == 3)
                secondTeamThirdTimeOut.Background = Brushes.Red;

            TimeOutTimer(s);
        }
        #endregion

        #region Button clicks (for table editing: incrementing and decrementing.
        private void btnAssistsUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.Assists);
        }
        private void btnAssistsDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.Assists);
        }
        private void btnReboundsUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.Rebound);
        }
        private void btnReboundsDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.Rebound);
        }
        private void btnStealsUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.Steals);
        }
        private void btnStealsDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.Steals);
        }
        private void btnBlockedShotsUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.BlockedShots);
        }
        private void btnBlockedShotsDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.BlockedShots);
        }
        private void btnShotsFromGameUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.ShotsFromGame);
        }
        private void btnShotsFromGameDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.ShotsFromGame);
        }
        private void btnShotsFromGameSuccessfullUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.ShotsFromGameSuccessfull);
        }
        private void btnShotsFromGameSuccessfullDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.ShotsFromGameSuccessfull);
        }
        private void btnShotsFromGameFarUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.ShotsFromGameFar);
        }
        private void btnShotsFromGameFarDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.ShotsFromGameFar);
        }
        private void btnShotsFromGameFarSuccessfullUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.ShotsFromGameFarSuccessfull);
        }
        private void btnShotsFromGameFarSuccessfullDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.ShotsFromGameFarSuccessfull);
        }
        private void btnFreeThrowsUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.FreeTrows);
        }
        private void btnFreeThrowsDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.FreeTrows);
        }
        private void btnFreeThrowsSuccessfullUp_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(1, StatisticItem.FreeTrowsSuccessfull);
        }
        private void btnFreeThrowsSuccessfullDown_Click(object sender, RoutedEventArgs e)
        {
            DataGridChange(-1, StatisticItem.FreeTrowsSuccessfull);
        }
        #endregion

        private void DataGridChange(int change, StatisticItem statistic)
        {
            try
            {
                // Getting index and a new player with changes.
                var index = dataGridPlayers.SelectedIndex;
                _gameRepository.ChangeStat(dataGridPlayers.SelectedItem, statistic, change);
                // Adding changes to the DataGrid.
                dataGridPlayers.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}