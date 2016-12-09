using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStatistics.Data
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Height { get; set; }
        public double Mass { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public Team Team { get; set; }
    }
}
