using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BasketballStatistics.Data
{
    public class Repository
    {
        Context context;
        List<Player> _allPlayers;

        public async Task<IEnumerable<Team>> GetAllTeams()
        {
            using (context = new Context())
                return await context.Teams.ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            if (_allPlayers == null)
                using (context = new Context())
                    _allPlayers = await context.Players.Include(p => p.Team)
                                            .OrderBy(p => p.Surname)
                                            .ToListAsync();
            return _allPlayers;
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

        public Match FindMatch(object selected)
        {
            if (!(selected is MatchViewModel))
                throw new ArgumentException("Something went wrong! Try again.");
            var model = selected as MatchViewModel;

            using (context = new Context())
                return context.Matches
                        .Include(m => m.Team1)
                        .Include(m => m.Team2)
                        .First(match => match.Team1.Name == model.FirstTeam
                            && match.Team2.Name == model.SecondTeam
                            && match.Date == model.Date);
        }

        public Player FindPlayer(string name, string surname, double height, double weight, DateTime birthdate, Position position)
        {
            using (context = new Context())
                return context.Players.FirstOrDefault(pl => pl.Name == name && pl.Surname == surname
                                                      && pl.Height == height && pl.Weight == weight
                                                      && pl.BirthDate == birthdate && pl.Position == position);
        }

        public string FindCoach(Team team)
        {
            using (context = new Context())
            {
                var coach = (context.Coaches.Where(c => c.Team.Id == team.Id)).First();
                return coach.Name + " " + coach.Surname;
            }
        }

        //Расчет простой статистики команд по победам/поражениям
        public IEnumerable<ShortCommandStatisticsViewModel> ShortStatisticsOfTeams()
        {
            using (context = new Context())
            {
                // At first we get all winners.
                var winners = context.Matches.Select(match => new
                {
                    Team = match.Team1Score > match.Team2Score ? match.Team1 : match.Team2,
                    Win = 1,
                    Lose = 0
                });
                // Then we get all losers.
                var losers = context.Matches.Select(match => new
                {
                    Team = match.Team1Score > match.Team2Score ? match.Team2 : match.Team1,
                    Win = 0,
                    Lose = 1
                });
                // We concat them and group by team. So we get team, its loses and victories.
                return (from t in winners.Concat(losers)
                        group t by t.Team into g
                        orderby g.Key.Name
                        select new ShortCommandStatisticsViewModel
                        {
                            Name = g.Key.Name,
                            Wins = g.Sum(result => result.Win),
                            Loses = g.Sum(result => result.Lose)
                        }).ToList();
            }
        }

        //Расчет персональных статистик игроков определенной команды в определенном матче (нужно в окне AllGames)
        public IEnumerable<PersonalStatistics> StatisticsOfPlayersOfTeam(Match match)
        {
            using (context = new Context())
                return (from stat in context.PersonalStatistics
                                        .Include(p => p.Player)
                                        .Include(p => p.Match.Team2)
                                        .Include(p => p.Match.Team1)
                        where stat.Match.Id == match.Id
                        select stat).ToList();
        }

        // Добавление команды в базу. Возврат : Exception - команда уже существует
        public void AddTeamInDatabase(string nameOfTeam, string coachName, string coachSurname)
        {
            if (string.IsNullOrWhiteSpace(nameOfTeam))
                throw new ArgumentException("Team name cannot be null or whitespaces!");
            if (string.IsNullOrWhiteSpace(coachName))
                throw new ArgumentException("Coach name cannot be null or whitespaces!");
            if (string.IsNullOrWhiteSpace(coachSurname))
                throw new ArgumentException("Coach surname cannot be null or whitespaces!");

            if (FindTeam(nameOfTeam) != null)
                throw new ArgumentException("Team with this name already exists!");

            using (context = new Context())
            {
                var team = new Team { Name = nameOfTeam };
                context.Teams.Add(team);
                context.Coaches.Add(new Coach { Name = coachName, Surname = coachSurname, Team = team });
                context.SaveChanges();
            }
        }

        //Добавление игрока в базу данных. Возврат : Exception- игрок уже существует
        public void AddPlayerInDatabase(string name, string surname, double height, double weight, DateTime birthdate, string position, Team team)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or whitespaces");
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Surname cannot be null or whitespaces");
            if (birthdate == null)
                throw new ArgumentException("Incorrect format of BirthDate");
            if (birthdate > DateTime.Now)
                throw new ArgumentException("BirthDate cannot be later than now");
            if ((DateTime.Now - birthdate).Days / 365 < 15)
                throw new ArgumentException("Player cannot be younger than 15 years");
            var positionEnum = new Position();
            switch (position)
            {
                case "Point Guard":
                    positionEnum = Position.PointGuard;
                    break;
                case "Shooting Guard":
                    positionEnum = Position.ShootingGuard;
                    break;
                case "Small Forward":
                    positionEnum = Position.SmallForward;
                    break;
                case "Power Forward":
                    positionEnum = Position.PowerForward;
                    break;
                case "Center":
                    positionEnum = Position.Center;
                    break;
            }

            if (FindPlayer(name, surname, height, weight, birthdate, positionEnum) != null)
                throw new ArgumentException("This player alredy exists!");

            using (context = new Context())
            {
                context.Players.Add(new Player
                {
                    Name = name,
                    Surname = surname,
                    BirthDate = birthdate,
                    Height = height,
                    Weight = weight,
                    Position = positionEnum,
                    Team = context.Teams.First(t => t.Id == team.Id)
                });
                context.SaveChanges();
            }
        }

        public void DeletePlayerFromDatabase(object selectedPlayer)
        {
            var player = selectedPlayer as Player;
            if (player == null)
                throw new ArgumentException("Something went wrong!");

            _allPlayers.Remove(player);

            using (context = new Context())
            {
                var ps = context.PersonalStatistics.Where(stat => stat.Player.Id == player.Id);
                context.PersonalStatistics.RemoveRange(ps);
                context.Players.Remove(context.Players.First(p => p.Id == player.Id));
                context.SaveChanges();
            }
        }
    }
}