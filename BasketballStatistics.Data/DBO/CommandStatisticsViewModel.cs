using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStatistics.Data
{
    public class CommandStatisticsViewModel
    {
        public int Points { get; set; }
        public int Assists { get; set; }
        public int Rebounds { get; set; }
        public int Steals { get; set; }
        public int BlockedShots { get; set; }
        public int ShotsFromGame { get; set; }
        public int ShotsFromGameSuccessfull { get; set; }
        public string ShotsFromGamePercent { get; set; }
        public int ShotsFromGameFar { get; set; }
        public int ShotsFromGameFarSuccessfull { get; set; }
        public string ShotsFromGameFarPercent { get; set; }
    }
}
