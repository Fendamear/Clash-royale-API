using System.Security.Policy;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.DTOs.Clan
{
    public class GetClanMemberLogDTO
    {
        public Guid Guid { get; set; }

        public string Tag { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        public MemberStatus Status { get; set; }

        public string Name { get; set; } = string.Empty;

        public string OldValue { get; set; } = string.Empty;

        public string NewValue { get; set; } = string.Empty;  
    }
}
