using System;
using System.Collections.Generic;

namespace BayraktarGame
{
    [Serializable]
    public class User 
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PassWord { get; set; }
        public virtual List<Statistic> Statistics { get; set; }
        public override bool Equals(object obj)
        {
            if (!(obj is User user))
                return false;
            return user.Login.Equals(Login) && user.PassWord.Equals(PassWord);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Login != null ? Login.GetHashCode() : 0) * 397) ^ (PassWord != null ? PassWord.GetHashCode() : 0);
            }
        }
    }
}