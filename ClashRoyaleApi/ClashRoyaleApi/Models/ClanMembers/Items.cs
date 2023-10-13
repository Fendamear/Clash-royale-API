using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashRoyaleApi.Models.ClanMembers
{
    public class Items
    {
        public string? Tag { get; set; }

        public string? Name { get; set; }

        public string? Role { get; set; }

        public DateTime LastSeen { get; set; }

        public int ExpLevel { get; set; }

        public Arena Arena { get; set; } = new Arena();

    }
}
