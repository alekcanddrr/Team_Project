using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStatistics.Data
{
    public class Repository
    {
        Context context;

        public IEnumerable<Team> AllTeams
        {
            get
            {
                using (context = new Context())
                    return context.Teams.ToList();
            }
        }

        public IEnumerable<MatchViewModel> AllMatchesData()
        {
            using (context = new Context())
                return (from match in context.Matches
                        select new MatchViewModel
                        {
                            FirstTeam = match.Team1.Name,
                            SecondTeam = match.Team2.Name,
                            Score = match.Team1Score.ToString() + ":" + match.Team2Score.ToString(),
                            Date = match.Date
                        }).ToList();
        }

        public Team FindTeam(string name)
        {
            using (context = new Context())
                return context.Teams.FirstOrDefault(t => t.Name == name);
        }

        public Match FindMatch(string firstTeam, string secondTeam, string finalScore)
        {
            using (context = new Context())
            {
                var search = from match in context.Matches
                             where (match.Team1.Name == firstTeam && match.Team2.Name == secondTeam && match.Team1Score == int.Parse(finalScore.Split(':')[0]) && match.Team2Score == int.Parse(finalScore.Split(':')[1]))
                             select match;
                var result = search.First();
                return result;
            }
        }
        //Определение есть ли уже такой игрок в БД. Error - игрок есть, Ok - все нормально
        public Player FindPlayer(string _name, string _surname, double _height, double _weight, int _age, Position _position)
        {
            using (context = new Context())
                return context.Players.FirstOrDefault(pl => pl.Name == _name && pl.Surname == _surname
                                                      && pl.Height == _height && pl.Weight == _weight
                                                      && pl.Age == _age && pl.Position == _position); 
        }

        public CommandStatisticsViewModel GameStatisticsOfTeam(Match match, string nameOfTeam)
        {
            using (context = new Context())
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
                                 ShotsFromGamePercent = stat.ShotsFromGame == 0 ? "None" : (((double)stat.ShotsFromGameSuccessfull / stat.ShotsFromGame) * 100).ToString("#.##") + "%",
                                 ShotsFromGameFar = stat.ShotsFromGameFar,
                                 ShotsFromGameFarSuccessfull = stat.ShotsFromGameFarSuccessfull,
                                 ShotsFromGameFarPercent = stat.ShotsFromGameFar == 0 ? "None" : (((double)stat.ShotsFromGameFarSuccessfull / stat.ShotsFromGameFar) * 100).ToString("#.##") + "%",
                                 FreeThrows = stat.FreeThrows,
                                 FreeThrowsSuccessfull = stat.FreeThrowsSuccessfull,
                                 FreeThrowsPercent = stat.FreeThrows == 0 ? "None" : (((double)stat.FreeThrowsSuccessfull / stat.FreeThrows) * 100).ToString("#.##") + "%"
                             };
                return result.First();
            }
        }

        //Расчет простой статистики команд по победам/поражениям
        public IEnumerable<ShortCommandStatisticsViewModel> ShortStatisticsOfTeams()
        {
            List<ShortCommandStatisticsViewModel> result = new List<ShortCommandStatisticsViewModel>();
            using (context = new Context())
            {
                var teams = (from team in context.Teams
                             select team).ToList();
                foreach (var team in teams)
                {
                    var wins = 0;
                    var loses = 0;
                    var statistics = (from stat in context.CommandStatistics
                                      where stat.Team.Name == team.Name
                                      select stat).ToList();
                    foreach (var stats in statistics)
                    {
                        if (stats.Match.Team1 == team)
                        {
                            if (stats.Match.Team1Score > stats.Match.Team2Score) wins++;
                            else loses++;
                        }
                        else
                            if (stats.Match.Team2 == team)
                        {
                            if (stats.Match.Team1Score > stats.Match.Team2Score) loses++;
                            else wins++;
                        }
                    }
                    result.Add(new ShortCommandStatisticsViewModel { Name = team.Name, Wins = wins, Loses = loses });
                }
                return result;
            }
        }

        //Расчет персональных статистик игроков определенной команды в определенном матче (нужно в окне AllGames)
        public IEnumerable<PersonalStatisticsViewModel> StatisticsOfPlayersOfTeam(Match match, string _nameOfTeam)
        {
            using (context = new Context())
            {
                var playersStat = (from stat in context.PersonalStatistics
                                   where stat.Player.Team.Name == _nameOfTeam
                                   select new PersonalStatisticsViewModel
                                   {
                                       Name = stat.Player.Name,
                                       Surname = stat.Player.Surname,
                                       Position = stat.Player.Position,
                                       Points = stat.Points,
                                       Assists = stat.Assists,
                                       Rebounds = stat.Rebounds,
                                       Steals = stat.Steals,
                                       BlockedShots = stat.BlockedShots,
                                       ShotsFromGame = stat.ShotsFromGame,
                                       ShotsFromGameSuccessfull = stat.ShotsFromGameSuccessfull,
                                       ShotsFromGamePercent = stat.ShotsFromGame == 0 ? "None" : (((double)stat.ShotsFromGameSuccessfull / stat.ShotsFromGame) * 100).ToString("#.##") + "%",
                                       ShotsFromGameFar = stat.ShotsFromGameFar,
                                       ShotsFromGameFarSuccessfull = stat.ShotsFromGameFarSuccessfull,
                                       ShotsFromGameFarPercent = stat.ShotsFromGameFar == 0 ? "None" : (((double)stat.ShotsFromGameFarSuccessfull / stat.ShotsFromGameFar) * 100).ToString("#.##") + "%",
                                       FreeThrows = stat.FreeThrows,
                                       FreeThrowsSuccessfull = stat.FreeThrowsSuccessfull,
                                       FreeThrowsPercent = stat.FreeThrows == 0 ? "None" : (((double)stat.FreeThrowsSuccessfull / stat.FreeThrows) * 100).ToString("#.##") + "%"
                                   }).ToList();
                return playersStat;
            }

        }

        // Добавление команды в базу. Возврат : Error - команда уже существует, Ok - все хорошо
        public string AddTeamInDatabase(string _nameOfTeam)
        {
            var checkIfExists = FindTeam(_nameOfTeam);

            if (checkIfExists != null)
                return "Error";
            else
            {
                using (context = new Context())
                {
                    context.Teams.Add(new Team { Name = _nameOfTeam });
                    context.SaveChanges();
                }
                return "Ok";
            }
        }
        //Добавление игрока в базу данных. Возврат : Error - игрок уже существует, Ok - все хорошо
        public string AddPlayerInDatabase(string _name, string _surname, double _height, double _weight, int _age, string _position, Team team)
        {
            var position = new Position();
            switch (_position)
            {
                case "Point Guard":
                    position = Position.PointGuard;
                    break;
                case "Shooting Guard":
                    position = Position.ShootingGuard;
                    break;
                case "Small Forward":
                    position = Position.SmallForward;
                    break;
                case "Power Forward":
                    position = Position.PowerForward;
                    break;
                case "Center":
                    position = Position.Center;
                    break;
            }

            if (FindPlayer(_name, _surname, _height, _weight, _age, position) != null)
                return "Error";
            else
            {
                var _player = new Player { Name = _name, Surname = _surname, Age = _age, Height = _height, Weight = _weight, Position = position, Team = team };
                using (context = new Context())
                {
                    context.Players.Add(_player);
                    context.SaveChanges();
                }
                return "Ok";
            }
        }
    }
}
