﻿using System;
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
                return context.Matches.First(match =>
                    match.Team1.Name == firstTeam && match.Team2.Name == secondTeam
                    && match.Team1Score == int.Parse(finalScore.Split(':')[0])
                    && match.Team2Score == int.Parse(finalScore.Split(':')[1]));
            }
        }

        public Player FindPlayer(string name, string surname, double height, double weight, DateTime birthdate, Position position)
        {
            using (context = new Context())
                return context.Players.FirstOrDefault(pl => pl.Name == name && pl.Surname == surname
                                                      && pl.Height == height && pl.Weight == weight
                                                      && pl.BirthDate == birthdate && pl.Position == position);
        }

        public CommandStatisticsViewModel GameStatisticsOfTeam(Match match, string nameOfTeam)
        {
            using (context = new Context())
                return (from stat in context.CommandStatistics
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
                        }).First();
        }

        //Расчет простой статистики команд по победам/поражениям
        public IEnumerable<ShortCommandStatisticsViewModel> ShortStatisticsOfTeams()
        {
            using (context = new Context())
            {
                var winners = context.Matches.Select(match => new
                {
                    Team = match.Team1Score > match.Team2Score ? match.Team1 : match.Team2,
                    Win = 1,
                    Lose = 0
                });
                var losers = context.Matches.Select(match => new
                {
                    Team = match.Team1Score > match.Team2Score ? match.Team2 : match.Team1,
                    Win = 0,
                    Lose = 1
                });

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
        public IEnumerable<PersonalStatisticsViewModel> StatisticsOfPlayersOfTeam(Match match, string nameOfTeam)
        {
            using (context = new Context())
                return (from stat in context.PersonalStatistics
                        where stat.Player.Team.Name == nameOfTeam && stat.Match.Id == match.Id
                        select new PersonalStatisticsViewModel
                        {
                            Name = stat.Player.Name,
                            Surname = stat.Player.Surname,
                            Position = stat.Player.Position.ToString(),
                            Age = ((int)(DateTime.Now - stat.Player.BirthDate).TotalDays) / 365,
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
        }

        // Добавление команды в базу. Возврат : Exception - команда уже существует
        public Team AddTeamInDatabase(string nameOfTeam)
        {
            var checkIfExists = FindTeam(nameOfTeam);

            if (checkIfExists != null)
                throw new ArgumentException("Team with this name already exists!");
            else
            {
                using (context = new Context())
                {
                    var team = new Team { Name = nameOfTeam };
                    context.Teams.Add(team);
                    context.SaveChanges();
                    return team;
                }
            }
        }
        //Добавление игрока в базу данных. Возврат : Exception- игрок уже существует
        public void AddPlayerInDatabase(string name, string surname, double height, double weight, DateTime birthdate, string position, Team team)
        {
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
            else
            {
                var player = new Player { Name = name, Surname = surname, BirthDate = birthdate, Height = height, Weight = weight, Position = positionEnum, Team = team };
                using (context = new Context())
                {
                    context.Players.Add(player);
                    context.SaveChanges();
                }
            }
        }
        public void AddCoachInDatabase(string name, string surname, Team team)
        {
            using (context = new Context())
            {
                context.Coaches.Add(new Coach { Name = name, Surname = surname, Team = team });
            }
        }
    }
}
