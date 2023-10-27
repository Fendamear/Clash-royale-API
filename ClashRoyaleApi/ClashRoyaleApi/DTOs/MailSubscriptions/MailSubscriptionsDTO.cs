using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.DTOs.MailSubscriptions
{
    public class MailSubscriptionsDTO
    {
        public MailType MailType { get; set; }

        public SchedulerTime SchedulerTime { get; set; }

        public bool IsEnabled { get; set; } 

    }
}
