using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Models.DbModels
{
    public class DbClanMemberLog
    {
        [Key]
        public Guid Guid { get; set; }

        public string Tag { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        public string OldValue { get; set; } = string.Empty;

        public string NewValue { get; set; } = string.Empty;

        public MemberStatus Status { get; set; }    

    }
}
