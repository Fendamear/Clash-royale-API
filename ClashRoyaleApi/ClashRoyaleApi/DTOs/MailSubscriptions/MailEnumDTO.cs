using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.DTOs.MailSubscriptions
{
    public class MailEnumDTO
    {
        public MailType type { get; set; }

        public SchedulerTime time { get; set; } 

        public MailEnumDTO() { }    
    }
}
