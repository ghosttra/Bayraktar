namespace BayraktarGame
{
    public class Statistic
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int UserId { get; set; }
        public virtual User Users { get; set; }
    }
}