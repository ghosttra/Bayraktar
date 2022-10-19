using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BayraktarGame
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PassWord { get; set; }
        public virtual List<Statistic> Statistics { get; set; }
    }
    public class Statistic
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int UserId { get; set; }
        public virtual User Users { get; set; }
    }

    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public byte[] ImageDestroyed { get; set; }
        public int CoolDown { get; set; }
        public int Price { get; set; }
    }
}
