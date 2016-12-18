﻿using System;
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

        private DispatcherTimer GameTime = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private DispatcherTimer TimeOut = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private DateTime StartTime;
        private TimeSpan TimerGame = TimeSpan.FromMinutes(10);
        private DateTime StartTimeOut = DateTime.Now;
        private TimeSpan TimerTimeOut = TimeSpan.FromMinutes(1);
        private DateTime PauseTime;
        private int Quarter = 1;
        private bool play;

        public NewGameWindow(GameType gameType, Team team1, Team team2, string matchPlace)
        {
            InitializeComponent();
            GameTime.Tick += TimeTick;
            TimeOut.Tick += TimeOutTimer_Tick;

            _repository = new Repository();
            _gameRepository = new RepositoryGame(team1, team2, matchPlace);

            txtFirstTeam.Text = team1.Name;
            txtSecondTeam.Text = team2.Name;
            txtFirstTeamCoach.Text = "Coach: " + _repository.FindCoach(team1);
            txtSecondTeamCoach.Text = "Coach: " + _repository.FindCoach(team2);

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

        private void TimeOutTimer(string TeamTimeOut)
        {
            TimerPause();

            txtQuarter.Text = TeamTimeOut;
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = false;
            btnFirstTeamTimeOut.IsEnabled = false;
            btnSecondTeamTimeOut.IsEnabled = false;

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
                btnStop.IsEnabled = false;
                btnFirstTeamTimeOut.IsEnabled = true;
                btnSecondTeamTimeOut.IsEnabled = true;
                txtQuarter.Text = Quarter.ToString();
                TimeLeft = TimerGame - (DateTime.Now - StartTime.Add(DateTime.Now - PauseTime));
                txtTime.Text = TimeLeft.Minutes + ":" + TimeLeft.Seconds;
            }
            else
                txtTime.Text = TimeLeft.Minutes + ":" + TimeLeft.Seconds;
        }

        private void btnFirstTeamTimeOut_Click(object sender, RoutedEventArgs e)
        {
            string s = "Time Out by 1 team";
            TimeOutTimer(s);
        }

        private void btnSecondTeamTimeOut_Click(object sender, RoutedEventArgs e)
        {
            string s = "Time Out by 2 team";
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