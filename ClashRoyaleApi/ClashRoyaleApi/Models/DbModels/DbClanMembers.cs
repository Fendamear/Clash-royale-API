using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashRoyaleApi.Models.DbModels
{
    public class DbClanMembers
    {
        [Key]
        public Guid Guid { get; set; }

        public string ClanTag { get; set; } 

        public string Name { get; set; }    

        public string Role { get; set; }    

        public DateTime LastSeen { get; set; }

        public bool IsActive { get; set; }  

        public bool IsInClan { get; set; }

    }
}
