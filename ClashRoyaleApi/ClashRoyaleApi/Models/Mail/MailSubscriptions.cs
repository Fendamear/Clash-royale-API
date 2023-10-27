using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Models.Mail
{
    public class MailSubscriptions
    {
        [Key]
        public Guid Guid { get; set; }

        public MailType MailType { get; set; }

        public SchedulerTime? SchedulerTime { get; set; }

        [ForeignKey("dbuser")]
        public string ClanTag { get; set; }

        public MailSubscriptions()
        {

        }

        public MailSubscriptions(MailType type, SchedulerTime time, string clantag)
        {
            Guid = Guid.NewGuid();
            MailType = type;
            SchedulerTime = time;
            ClanTag = clantag;
        }




    }
}
