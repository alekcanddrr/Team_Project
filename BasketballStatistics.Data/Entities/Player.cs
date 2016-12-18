using System;

namespace BasketballStatistics.Data
{
    public enum Position
    {
        PointGuard,
        ShootingGuard,
        Center,
        SmallForward,
        PowerForward
    }
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public DateTime BirthDate { get; set; }
        public Position Position { get; set; }
        public virtual Team Team { get; set; }
    }
}
