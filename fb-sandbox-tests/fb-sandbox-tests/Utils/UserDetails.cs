using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbsandboxtests.Utils
{
    public class UserDetails
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Balance { get; set; }

        public override string ToString()
        {
            return Id +"|" + Name + "|" + Balance;
        }
    }
}
