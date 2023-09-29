using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClashRoyaleCodeBase.Models.EnumClass;

namespace ClashRoyaleCodeBase.Models.DbModels
{
    public class DbClanMemberLog
    {
        [Key]
        public Guid Guid { get; set; }

        public string Tag { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }

        public MemberStatus Status { get; set; }    

    }
}
