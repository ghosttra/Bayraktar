using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BayraktarGame;

namespace GameEntities
{
    public class GameContext:DbContext
    {
        public GameContext() : base("BayraktarDataBase")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Unit> Units { get; set; }
    }
}
