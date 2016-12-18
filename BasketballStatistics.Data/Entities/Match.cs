using System;

namespace BasketballStatistics.Data
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

    }
}