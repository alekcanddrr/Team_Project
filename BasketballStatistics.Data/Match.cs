using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStatistics.Data
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public string FinalScore { get; set; }

    }
}
