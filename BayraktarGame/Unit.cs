using System;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BayraktarGame
{
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
