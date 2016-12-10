using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStatistics.Data
{
    public class Repository
    {
        public IQueryable<MatchViewModel> AllMatchesData(Context context)
        {
            var result = from match in context.Matches
                         select new MatchViewModel
                         {
                             FirstTeam = match.Team1.Name,
                             SecondTeam = match.Team2.Name,
                             Score = match.Team1Score.ToString() + ":" + match.Team2Score.ToString()
                         };
            return result;
        }

        //public Team FindTeam(Context context, string Name)
        //{
        //    var teams = from team in context.Teams
        //                 where team.Name == Name
        //                 select team;
        //    var result = teams.First();
        //    return result;
        //}
        public IQueryable<string> TeamBoxLoadWithData(Context context)
        {
            var result = from team in context.Teams
                         select team.Name;
            return result;
        }
        public Match FindMatch(Context context, string firstTeam, string secondTeam, string finalScore)
        {
            var search = from match in context.Matches
                         where (match.Team1.Name == firstTeam && match.Team2.Name == secondTeam && match.Team1Score == int.Parse(finalScore.Split(':')[0]) && match.Team2Score == int.Parse(finalScore.Split(':')[1]))
                         select match;
            var result = search.First();
            return result;
        }

        public CommandStatisticsViewModel GameStatisticsOfTeam(Context context, Match match, string nameOfTeam)
        {
            var result = from stat in context.CommandStatistics
                         where (stat.Match == match) && (stat.Match.Team1.Name == nameOfTeam || stat.Match.Team2.Name == nameOfTeam)
                         select new CommandStatisticsViewModel
                         {
                             Points = stat.Points,
                             Assists = stat.Assists,
                             Rebounds = stat.Rebounds,
                             Steals = stat.Steals,
                             BlockedShots = stat.BlockedShots,
                             ShotsFromGame = stat.ShotsFromGame,
                             ShotsFromGameSuccessfull = stat.ShotsFromGameSuccessfull,
                             ShotsFromGamePercent = (((double)stat.ShotsFromGameSuccessfull / stat.ShotsFromGame) * 100).ToString("#.##") + "%",
                             ShotsFromGameFar = stat.ShotsFromGameFar,
                             ShotsFromGameFarSuccessfull = stat.ShotsFromGameFarSuccessfull,
                             ShotsFromGameFarPercent = (((double)stat.ShotsFromGameFarSuccessfull / stat.ShotsFromGameFar) * 100).ToString("#.##") + "%"
                         };
            return result.First();
        }

        public List<ShortCommandStatisticsViewModel> ShortStatisticsOfTeams(Context context)
        {
            List<ShortCommandStatisticsViewModel> result = new List<ShortCommandStatisticsViewModel>();
            var teams = from team in context.Teams
                        select team;
            foreach (var team in teams)
            {
                var wins = 0;
                var loses = 0;
                var statistics = from stat in context.CommandStatistics
                                 where stat.Team == team
                                 select stat;
                foreach (var stats in statistics)
                {
                    if (stats.Match.Team1 == team)
                    {
                        if (stats.Match.Team1Score > stats.Match.Team2Score) wins++;
                        if (stats.Match.Team1Score < stats.Match.Team2Score) loses++;
                    }else
                        if (stats.Match.Team2 == team)
                    {
                        if (stats.Match.Team1Score > stats.Match.Team2Score) loses++;
                        if (stats.Match.Team1Score < stats.Match.Team2Score) wins++;
                    }
                }
                result.Add(new ShortCommandStatisticsViewModel { Name = team.Name, Wins = wins, Loses = loses });
            }
            return result;
        }
    }
}
