using System.Collections.Generic;

namespace BayraktarGame
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PassWord { get; set; }
        public virtual List<Statistic> Statistics { get; set; }
    }
}