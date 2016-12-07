using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStatistics.Data
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Player> Players { get; set; }
        public virtual List<Coach> Coaches { get; set; }
        public virtual List<Statistics> CommandStatistics { get; set; }
    }
}
