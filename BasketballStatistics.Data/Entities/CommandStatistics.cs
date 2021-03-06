﻿namespace BasketballStatistics.Data
{
    public class CommandStatistics
    {
        public int Id { get; set; }
        public virtual Match Match { get; set; }
        public virtual Team Team { get; set; }
        public int Points { get; set; }
        public int Assists { get; set; }
        public int Rebounds { get; set; }
        public int Steals { get; set; }
        public int BlockedShots { get; set; }
        public int ShotsFromGame { get; set; }
        public int ShotsFromGameSuccessfull { get; set; }
        public int ShotsFromGameFar { get; set; }
        public int ShotsFromGameFarSuccessfull { get; set; }
        public int FreeThrows { get; set; }
        public int FreeThrowsSuccessfull { get; set; }
    }
}
