using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BasketballStatistics.Data
{
    public enum StatisticItem
    {
        Points,
        Assists,
        Rebound,
        Steals,
        BlockedShots,
        ShotsFromGame,
        ShotsFromGameSuccessfull,
        ShotsFromGameFar,
        ShotsFromGameFarSuccessfull,
        FreeTrows,
        FreeTrowsSuccessfull
    }

    public class RepositoryGame
    {
        Context context;
        List<PersonalStatistics> stat;
        CommandStatistics statTeam1, statTeam2;
        Match match;

        public IEnumerable<PersonalStatistics> Statistics
        {
            get
            {
                return stat.ToList();
            }
        }

        public string Date
        {
            get
            {
                return match.Date.ToString("dd.MM.yyyy");
            }
        }

        public string Score
        {
            get
            {
                return $"{statTeam1.Points}:{statTeam2.Points}";
            }
        }

        public RepositoryGame(Team team1, Team team2, string place, DateTime date = default(DateTime))
        {
            if (string.IsNullOrWhiteSpace(place))
                throw new ArgumentException("Incorrect format of the place. This field should not be empty!");

            context = new Context();
            team1 = context.Teams.First(t => t.Id == team1.Id);
            team2 = context.Teams.First(t => t.Id == team2.Id);

            match = new Match
            {
                Date = date == default(DateTime) ? DateTime.Now : date,
                Place = place,
                Team1 = team1,
                Team2 = team2
            };

            statTeam1 = new CommandStatistics { Team = team1, Match = match };
            statTeam2 = new CommandStatistics { Team = team2, Match = match };

            stat = new List<PersonalStatistics>();
            foreach (var player in context.Players.Include(p => p.Team))
                if (player.Team.Id == team1.Id || player.Team.Id == team2.Id)
                    stat.Add(new PersonalStatistics { Player = player, Match = match });
        }

        public void ChangeStat(object selected, StatisticItem statItem, int change)
        {
            if (!(selected is PersonalStatistics))
                throw new Exception("Something went wrong. Try again.");

            var player = selected as PersonalStatistics;
            bool isInFirstTeam = player.Player.Team == statTeam1.Team;

            switch (statItem)
            {
                case StatisticItem.Assists:
                    Check(change, player.Assists);
                    player.Assists += change;
                    if (isInFirstTeam)
                        statTeam1.Assists += change;
                    else
                        statTeam2.Assists += change;
                    break;

                case StatisticItem.BlockedShots:
                    Check(change, player.BlockedShots);
                    player.BlockedShots += change;
                    if (isInFirstTeam)
                        statTeam1.BlockedShots += change;
                    else
                        statTeam2.BlockedShots += change;
                    break;

                case StatisticItem.FreeTrows:
                    Check(change, player.FreeThrows);
                    player.FreeThrows += change;
                    if (isInFirstTeam)
                        statTeam1.FreeThrows += change;
                    else
                        statTeam2.FreeThrows += change;
                    break;

                case StatisticItem.FreeTrowsSuccessfull:
                    Check(change, player.BlockedShots);
                    player.FreeThrowsSuccessfull += change;
                    player.FreeThrows += change;
                    player.Points += change > 0 ? 1 : -1;
                    if (isInFirstTeam)
                    {
                        statTeam1.FreeThrowsSuccessfull += change;
                        statTeam1.FreeThrows += change;
                        statTeam1.Points += change > 0 ? 1 : -1;
                    }
                    else
                    {
                        statTeam2.FreeThrowsSuccessfull += change;
                        statTeam2.FreeThrows += change;
                        statTeam2.Points += change > 0 ? 1 : -1;
                    }
                    break;

                case StatisticItem.Rebound:
                    Check(change, player.Rebounds);
                    player.Rebounds += change;
                    if (isInFirstTeam)
                        statTeam1.Rebounds += change;
                    else
                        statTeam2.Rebounds += change;
                    break;

                case StatisticItem.ShotsFromGame:
                    Check(change, player.ShotsFromGame);
                    player.ShotsFromGame += change;
                    if (isInFirstTeam)
                        statTeam1.ShotsFromGame += change;
                    else
                        statTeam2.ShotsFromGame += change;
                    break;

                case StatisticItem.ShotsFromGameFar:
                    Check(change, player.ShotsFromGameFar);
                    player.ShotsFromGameFar += change;
                    if (isInFirstTeam)
                        statTeam1.ShotsFromGameFar += change;
                    else
                        statTeam2.ShotsFromGameFar += change;
                    break;

                case StatisticItem.ShotsFromGameFarSuccessfull:
                    Check(change, player.ShotsFromGameFarSuccessfull);
                    player.ShotsFromGameFarSuccessfull += change;
                    player.ShotsFromGameFar += change;
                    player.Points += change > 0 ? 3 : -3;
                    if (isInFirstTeam)
                    {
                        statTeam1.ShotsFromGameFarSuccessfull += change;
                        statTeam1.ShotsFromGameFar += change;
                        statTeam1.Points += change > 0 ? 3 : -3;
                    }
                    else
                    {
                        statTeam2.ShotsFromGameFarSuccessfull += change;
                        statTeam2.ShotsFromGameFar += change;
                        statTeam2.Points += change > 0 ? 3 : -3;
                    }
                    break;

                case StatisticItem.ShotsFromGameSuccessfull:
                    Check(change, player.ShotsFromGameSuccessfull);
                    player.ShotsFromGameSuccessfull += change;
                    player.ShotsFromGame += change;
                    player.Points += change > 0 ? 2 : -2;
                    if (isInFirstTeam)
                    {
                        statTeam1.ShotsFromGameSuccessfull += change;
                        statTeam1.ShotsFromGame += change;
                        statTeam1.Points += change > 0 ? 2 : -2;
                    }
                    else
                    {
                        statTeam2.ShotsFromGameSuccessfull += change;
                        statTeam2.ShotsFromGame += change;
                        statTeam2.Points += change > 0 ? 2 : -2;
                    }
                    break;

                case StatisticItem.Steals:
                    Check(change, player.Steals);
                    player.Steals += change;
                    if (isInFirstTeam)
                        statTeam1.Steals += change;
                    else
                        statTeam2.Steals += change;
                    break;
            }
        }

        private void Check(int change, int property)
        {
            if (property == 0 && change < 0)
                throw new ArgumentException("A property cannot be negative value.");
        }

        public void GameOver()
        {
            match.Team1Score = statTeam1.Points;
            match.Team2Score = statTeam2.Points;

            //match.Team1 = context.Teams.First(t => t.Id == statTeam1.Team.Id);
            //match.Team2 = context.Teams.First(t => t.Id == statTeam2.Team.Id);

            //statTeam1.Team = context.Teams.First(t => t.Id == statTeam1.Team.Id);
            //statTeam2.Team = context.Teams.First(t => t.Id == statTeam2.Team.Id);

            //stat.ForEach(s => s.Player = context.Players.First(p => p.Id == s.Player.Id));

            context.Matches.Add(match);
            context.CommandStatistics.AddRange(new[] { statTeam1, statTeam2 });
            context.PersonalStatistics.AddRange(stat);

            context.SaveChanges();
            context.Dispose();
        }
    }
}
