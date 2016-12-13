using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BasketballStatistics.Data
{
    enum StatisticItem
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

    class RepositoryGame
    {
        List<PersonalStatistics> stat = new List<PersonalStatistics>();
        CommandStatistics statTeam1 = new CommandStatistics();
        CommandStatistics statTeam2 = new CommandStatistics();
        Match match;
        
        public RepositoryGame(string team1, string team2, string place)
        {
            using (Context c = new Context())
            {
                var players = c.Players.ToList().FindAll(p => p.Team.Name == team1 || p.Team.Name == team2);

                players.ForEach(p => stat.Add(new PersonalStatistics { Player = p }));

                statTeam1.Team = c.Teams.First(t => t.Name == team1);
                statTeam2.Team = c.Teams.First(t => t.Name == team2);
                match = new Match { Date = DateTime.Now, Place = place, Team1 = statTeam1.Team, Team2 = statTeam2.Team };

                stat.ForEach(s => s.Match = match);
                statTeam1.Match = match;
                statTeam2.Match = match;
            }
        }

        public RepositoryGame(string team1, string team2, DateTime Date)
        {
            using (Context c = new Context())
            {
                match = c.Matches.ToList().Find(m => (m.Team1.Name == team1 && m.Team2.Name == team2) || (m.Team1.Name == team2 && m.Team2.Name == team1));

                //stat = c.PersonalStatistics.ToList().FindAll(s => s.Match == match);
                //statTeam1 = c.CommandStatistics.ToList().Find(s => s.Match==match && s.Team==match.Team1);
                //statTeam2 = c.CommandStatistics.ToList().Find(s => s.Match == match && s.Team == match.Team2);
            }
        }

        public IEnumerable<PersonalStatisticsViewModel> GetPlayersStat()
        {
            using (Context c = new Context())
            {
                return (from stat in c.PersonalStatistics
                        where (stat.Match == match)
                        select new PersonalStatisticsViewModel
                        {
                            Name = stat.Player.Name,
                            Surname = stat.Player.Surname
                            // надо остальное прописывать?
                        }
                        ).ToList();
            }
        }

        public IEnumerable<Player> GetTeamList(string team)
        {
            using (Context c = new Context())
            {
                return c.Players.ToList().FindAll(p => p.Team.Name == team);
            }
        }

        public IEnumerable<CommandStatisticsViewModel> GetTeamStat(string team)
        {
            using (Context c = new Context())
            {
                return (from tStat in c.CommandStatistics
                        where (tStat.Team.Name == team && tStat.Match == match)
                        select new CommandStatisticsViewModel()
                        //здесь тоже все прописывать? 
                        ).ToList();
            }
        }

        public void ChangeStat(Player player, StatisticItem statItem , bool f)
        {
            var p = stat.Find(s => s.Player.Name == player.Name && s.Player.Surname == player.Surname);

            if (f)
            {
                
                switch (statItem)
                {
                    case StatisticItem.Assists:
                        p.Assists++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.Assists++;
                        else
                            statTeam2.Assists++;
                        break;
                    case StatisticItem.BlockedShots:
                        p.BlockedShots++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.BlockedShots++;
                        else
                            statTeam2.BlockedShots++;
                        break;
                    case StatisticItem.FreeTrows:
                        p.FreeThrows++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.FreeThrows++;
                        else
                            statTeam2.FreeThrows++;
                        break;
                    case StatisticItem.FreeTrowsSuccessfull:
                        p.FreeThrowsSuccessfull++;
                        p.Points += 1;
                        if (statTeam1.Team == p.Player.Team)
                        {
                            statTeam1.FreeThrowsSuccessfull++;
                            statTeam1.Points += 1;
                        }
                        else
                        {
                            statTeam2.FreeThrowsSuccessfull++;
                            statTeam2.Points += 1;
                        }
                        break;
                    case StatisticItem.Rebound:
                        p.Rebounds++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.Rebounds++;
                        else
                            statTeam2.Rebounds++;
                        break;
                    case StatisticItem.ShotsFromGame:
                        p.ShotsFromGame++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.ShotsFromGame++;
                        else
                            statTeam2.ShotsFromGame++;
                        break;
                    case StatisticItem.ShotsFromGameFar:
                        p.ShotsFromGameFar++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.ShotsFromGameFar++;
                        else
                            statTeam2.ShotsFromGameFar++;
                        break;
                    case StatisticItem.ShotsFromGameFarSuccessfull:
                        p.ShotsFromGameFarSuccessfull++;
                        p.Points += 3;
                        if (statTeam1.Team == p.Player.Team)
                        {
                            statTeam1.ShotsFromGameFarSuccessfull++;
                            statTeam1.Points += 3;
                        }
                        else
                        {
                            statTeam2.ShotsFromGameFarSuccessfull++;
                            statTeam2.Points += 3;
                        }
                        break;
                    case StatisticItem.ShotsFromGameSuccessfull:
                        p.ShotsFromGameSuccessfull++;
                        p.Points += 2;
                        if (statTeam1.Team == p.Player.Team)
                        {
                            statTeam1.ShotsFromGameSuccessfull++;
                            statTeam1.Points += 2;
                        }
                        else
                        {
                            statTeam2.ShotsFromGameSuccessfull++;
                            statTeam2.Points += 2;
                        }
                        break;
                    case StatisticItem.Steals:
                        p.Steals++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.Steals++;
                        else
                            statTeam2.Steals++;
                        break;
                }
            }
            else
            {
                switch (statItem)
                {
                    case StatisticItem.Assists:
                        p.Assists--;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.Assists--;
                        else
                            statTeam2.Assists--;
                        break;
                    case StatisticItem.BlockedShots:
                        p.BlockedShots--;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.BlockedShots--;
                        else
                            statTeam2.BlockedShots--;
                        break;
                    case StatisticItem.FreeTrows:
                        p.FreeThrows--;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.FreeThrows--;
                        else
                            statTeam2.FreeThrows--;
                        break;
                    case StatisticItem.FreeTrowsSuccessfull:
                        p.FreeThrowsSuccessfull--;
                        p.Points -= 1;
                        if (statTeam1.Team == p.Player.Team)
                        {
                            statTeam1.FreeThrowsSuccessfull--;
                            statTeam1.Points-= 1;
                        }
                        else
                        {
                            statTeam2.FreeThrowsSuccessfull--;
                            statTeam2.Points -= 1;
                        }
                        break;
                    case StatisticItem.Rebound:
                        p.Rebounds--;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.Rebounds--;
                        else
                            statTeam2.Rebounds--;
                        break;
                    case StatisticItem.ShotsFromGame:
                        p.ShotsFromGame--;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.ShotsFromGame--;
                        else
                            statTeam2.ShotsFromGame--;
                        break;
                    case StatisticItem.ShotsFromGameFar:
                        p.ShotsFromGameFar--;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.ShotsFromGameFar--;
                        else
                            statTeam2.ShotsFromGameFar--;
                        break;
                    case StatisticItem.ShotsFromGameFarSuccessfull:
                        p.ShotsFromGameFarSuccessfull--;
                        p.Points-= 3;
                        if (statTeam1.Team == p.Player.Team)
                        {
                            statTeam1.ShotsFromGameFarSuccessfull--;
                            statTeam1.Points -= 3;
                        }
                        else
                        {
                            statTeam2.ShotsFromGameFarSuccessfull--;
                            statTeam2.Points -= 3;
                        }
                        break;
                    case StatisticItem.ShotsFromGameSuccessfull:
                        p.ShotsFromGameSuccessfull--;
                        p.Points += 2;
                        if (statTeam1.Team == p.Player.Team)
                        {
                            statTeam1.ShotsFromGameSuccessfull--;
                            statTeam1.Points -= 2;
                        }
                        else
                        {
                            statTeam2.ShotsFromGameSuccessfull--;
                            statTeam2.Points-= 2;
                        }
                        break;
                    case StatisticItem.Steals:
                        p.Steals++;
                        if (statTeam1.Team == p.Player.Team)
                            statTeam1.Steals--;
                        else
                            statTeam2.Steals--;
                        break;
                }
            }
        }

        public void GameOver()
        {
            using (Context c = new Context())
            {
                c.CommandStatistics.Add(statTeam1);
                c.CommandStatistics.Add(statTeam2);
                c.PersonalStatistics.AddRange(stat);
                c.Matches.Add(match);

                c.SaveChanges();
            }
        }

    }
}
