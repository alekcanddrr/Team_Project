using System.Data.Entity;

namespace BasketballStatistics.Data
{
    public class Context: DbContext
    {
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<PersonalStatistics> PersonalStatistics { get; set; }
        public DbSet<CommandStatistics> CommandStatistics { get; set; }
        public DbSet<Match> Matches { get; set; }

        public Context(): base("localsql")
        {

        }
    }
}
