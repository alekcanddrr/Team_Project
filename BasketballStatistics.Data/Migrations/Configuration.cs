namespace BasketballStatistics.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BasketballStatistics.Data.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BasketballStatistics.Data.Context context)
        {
            // Test information.

            List<Team> teams = new List<Team>
            {
                new Team { Name = "Lakers" },
                new Team { Name = "Big wolves" },
                new Team { Name = "Red socks" }
            };
            context.Teams.AddRange(teams);

            List<Coach> coaches = new List<Coach>
            {
                new Coach { Name = "Greg", Surname = "Popovic", Team = teams[0] },
                new Coach { Name = "Alexei", Surname = "Ivanov", Team = teams[1] },
                new Coach { Name = "Michael", Surname = "Jordan", Team = teams[2] }
            };
            context.Coaches.AddRange(coaches);

            List<Player> players = new List<Player>
            {
                   new Player {Name = "Mo", Surname = "Williams",Height = 180, Weight = 100, BirthDate = new DateTime(1980, 5,4), Position = Position.PointGuard, Team = teams[0]  },
                new Player {Name = "Dan", Surname = "Hickson",Height = 200, Weight = 110, BirthDate = new DateTime(1989, 3,1), Position = Position.PowerForward, Team = teams[0]  },
                new Player {Name = "Timofey", Surname = "Mozgov",Height = 225, Weight = 115, BirthDate = new DateTime(1990, 3,2), Position = Position.Center, Team = teams[0]  },
                new Player {Name = "Andrey", Surname = "Fillipov",Height = 195, Weight = 76, BirthDate = new DateTime(1995, 5,22), Position = Position.ShootingGuard, Team = teams[0]  },
                new Player {Name = "Lebron", Surname = "James",Height = 203, Weight = 100, BirthDate = new DateTime(1984,12,28), Position = Position.SmallForward, Team = teams[0]  },
                new Player {Name = "Tristan", Surname = "Tompson",Height = 210, Weight = 120,BirthDate = new DateTime(1999, 6,12), Position = Position.Center, Team = teams[0]  },
                new Player {Name = "Mathew", Surname = "Dellavedova",Height = 176, Weight =71, BirthDate = new DateTime(1997, 11,29), Position = Position.PointGuard, Team = teams[0]  },

                new Player {Name = "Wilcom", Surname = "Jackson",Height = 190, Weight = 80, BirthDate = new DateTime(1993, 1,2), Position = Position.PointGuard, Team = teams[1]  },
                new Player {Name = "Nicolai", Surname = "Tolstoi",Height = 196, Weight = 89, BirthDate = new DateTime(1993, 12,26), Position = Position.PowerForward, Team = teams[1]  },
                new Player {Name = "Andrew", Surname = "Ribakov",Height = 240, Weight = 145, BirthDate = new DateTime(1992, 5,2), Position = Position.Center, Team = teams[1]  },
                new Player {Name = "JR", Surname = "Smith",Height = 199, Weight = 82, BirthDate = new DateTime(1992, 7,24), Position = Position.ShootingGuard, Team = teams[1]  },
                new Player {Name = "Carmelo", Surname = "Anthony",Height = 204, Weight = 105, BirthDate = new DateTime(1999, 3,25), Position = Position.SmallForward, Team = teams[1]  },
                new Player {Name = "Jidrunas", Surname = "Ilgauskas",Height = 224, Weight = 127, BirthDate = new DateTime(1998, 1,14), Position = Position.Center, Team = teams[1]  },
                new Player {Name = "Derreck", Surname = "Rose",Height = 192, Weight =76, BirthDate = new DateTime(1993, 11,19), Position = Position.PointGuard, Team = teams[1]  },

                new Player {Name = "Leonard", Surname = "Smith",Height = 192, Weight = 95, BirthDate = new DateTime(1997, 1,1), Position = Position.PointGuard, Team = teams[2]  },
                new Player {Name = "Chris", Surname = "Bosh",Height = 214, Weight = 100, BirthDate = new DateTime(1996, 1,1), Position = Position.PowerForward, Team = teams[2]  },
                new Player {Name = "Ilya", Surname = "Smolyakov",Height = 237, Weight = 130, BirthDate = new DateTime(1992, 12,2), Position = Position.Center, Team = teams[2]  },
                new Player {Name = "Dwyane", Surname = "Wade",Height = 198, Weight = 88, BirthDate = new DateTime(1997, 10,14), Position = Position.ShootingGuard, Team = teams[2]  },
                new Player {Name = "Boris", Surname = "Brown",Height = 201, Weight = 100, BirthDate = new DateTime(1993, 11,24), Position = Position.SmallForward, Team = teams[2]  },
                new Player {Name = "Donald", Surname = "Trump",Height = 182, Weight = 89, BirthDate = new DateTime(1982, 3,28), Position = Position.Center, Team = teams[2]  },
                new Player {Name = "Stephen", Surname = "Curry",Height = 176, Weight =71, BirthDate = new DateTime(1992, 7,6), Position = Position.PointGuard, Team = teams[2]  }
            };
            context.Players.AddRange(players);

            List<Match> matches = new List<Match>
            {
                new Match {Date = DateTime.Now, Place = "Moscow", Team1 = teams[0], Team1Score = 98, Team2 = teams[1], Team2Score = 100 },
                new Match {Date = new DateTime(2015, 11, 10) , Place = "Chicago", Team1 = teams[0], Team1Score = 71, Team2 = teams[2], Team2Score = 55 },
                new Match {Date = new DateTime(2015, 11, 10) , Place = "Paris", Team1 = teams[1], Team1Score = 76, Team2 = teams[2], Team2Score = 42 }
            };
            context.Matches.AddRange(matches);

            List<PersonalStatistics> PersonalStatistics = new List<PersonalStatistics>
            {
                new PersonalStatistics {Match = matches[0], Player = players[8], Assists = 1, BlockedShots = 2, FreeThrows = 1, FreeThrowsSuccessfull = 1, Points = 5, Rebounds = 1, ShotsFromGame = 3, ShotsFromGameFar = 5, ShotsFromGameFarSuccessfull = 1, ShotsFromGameSuccessfull = 1, Steals = 1 },
                new PersonalStatistics {Match = matches[1], Player = players[15], Assists = 3, BlockedShots = 1, FreeThrows = 8, FreeThrowsSuccessfull = 2, Points = 4, Rebounds = 1, ShotsFromGame = 3, ShotsFromGameFar = 0, ShotsFromGameFarSuccessfull = 0, ShotsFromGameSuccessfull = 2, Steals = 2 },
                new PersonalStatistics {Match = matches[0], Player = players[10], Assists = 0, BlockedShots = 0, FreeThrows = 0, FreeThrowsSuccessfull = 0, Points = 3, Rebounds = 0, ShotsFromGame = 0, ShotsFromGameFar = 1, ShotsFromGameFarSuccessfull = 1, ShotsFromGameSuccessfull = 0, Steals = 4 },
                new PersonalStatistics {Match = matches[0], Player = players[8], Assists = 1, BlockedShots = 0, FreeThrows = 2, FreeThrowsSuccessfull = 2, Points = 11, Rebounds = 0, ShotsFromGame = 3, ShotsFromGameFar = 5, ShotsFromGameFarSuccessfull = 2, ShotsFromGameSuccessfull = 2, Steals = 0 },
                new PersonalStatistics {Match = matches[1], Player = players[4], Assists = 11, BlockedShots = 5, FreeThrows = 8, FreeThrowsSuccessfull = 6, Points = 38, Rebounds = 7, ShotsFromGame = 20, ShotsFromGameFar = 5, ShotsFromGameFarSuccessfull = 2, ShotsFromGameSuccessfull = 14, Steals = 8 },
                new PersonalStatistics {Match = matches[2], Player = players[14], Assists = 5, BlockedShots = 5, FreeThrows = 3, FreeThrowsSuccessfull = 3, Points = 7, Rebounds = 8, ShotsFromGame = 3, ShotsFromGameFar = 5, ShotsFromGameFarSuccessfull = 1, ShotsFromGameSuccessfull = 1, Steals = 1 },
                new PersonalStatistics {Match = matches[2], Player = players[17], Assists = 0, BlockedShots = 0, FreeThrows = 0, FreeThrowsSuccessfull = 0, Points = 9, Rebounds = 2, ShotsFromGame = 3, ShotsFromGameFar = 1, ShotsFromGameFarSuccessfull = 1, ShotsFromGameSuccessfull = 3, Steals = 2 }
            };
            context.PersonalStatistics.AddRange(PersonalStatistics);

            List<CommandStatistics> CommandStatistics = new List<CommandStatistics>
            {
                new CommandStatistics {Match = matches[0], Team = teams[0], Assists = 8, BlockedShots = 6, FreeThrows = 5, FreeThrowsSuccessfull = 1, Points = 98, Rebounds = 11, ShotsFromGame = 34, ShotsFromGameSuccessfull = 20, ShotsFromGameFar = 25, ShotsFromGameFarSuccessfull = 12, Steals = 11 },
                new CommandStatistics {Match = matches[0], Team = teams[1], Assists = 7, BlockedShots = 16, FreeThrows = 3, FreeThrowsSuccessfull = 3, Points = 100, Rebounds = 4, ShotsFromGame = 13, ShotsFromGameSuccessfull = 5, ShotsFromGameFar = 29, ShotsFromGameFarSuccessfull = 4, Steals = 8 },
                new CommandStatistics {Match = matches[1], Team = teams[2], Assists = 11, BlockedShots = 16, FreeThrows = 15, FreeThrowsSuccessfull = 12, Points = 55, Rebounds = 7, ShotsFromGame = 25, ShotsFromGameSuccessfull = 15, ShotsFromGameFar = 11, ShotsFromGameFarSuccessfull = 9, Steals = 3 },
                new CommandStatistics {Match = matches[1], Team = teams[0], Assists = 8, BlockedShots = 6, FreeThrows = 5, FreeThrowsSuccessfull = 1, Points = 71, Rebounds = 11, ShotsFromGame = 34, ShotsFromGameSuccessfull = 20, ShotsFromGameFar = 25, ShotsFromGameFarSuccessfull = 12, Steals = 11 },
                new CommandStatistics {Match = matches[2], Team = teams[1], Assists = 7, BlockedShots = 6, FreeThrows = 5, FreeThrowsSuccessfull = 1, Points = 76, Rebounds = 2, ShotsFromGame = 28, ShotsFromGameSuccessfull = 25, ShotsFromGameFar = 36, ShotsFromGameFarSuccessfull = 16, Steals = 15 },
                new CommandStatistics {Match = matches[2], Team = teams[2], Assists = 8, BlockedShots = 6, FreeThrows = 5, FreeThrowsSuccessfull = 1, Points = 42, Rebounds = 3, ShotsFromGame = 20, ShotsFromGameSuccessfull = 10, ShotsFromGameFar = 13, ShotsFromGameFarSuccessfull = 3, Steals = 10 }
            };
            context.CommandStatistics.AddRange(CommandStatistics);

            context.SaveChanges();
        }
    }
}
