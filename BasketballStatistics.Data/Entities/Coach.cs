namespace BasketballStatistics.Data
{
    public class Coach
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Team Team { get; set; }
    }
}
