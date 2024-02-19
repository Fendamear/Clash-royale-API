using System.ComponentModel.DataAnnotations;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Models.DbModels
{
    public class DbRiverRaceLog
    {
        [Key]
        public Guid Guid { get; set; }

        public DateTime TimeStamp { get; set; }

        public int SeasonId { get; set; }

        public int SectionId { get; set; }

        public PeriodType Type { get; set; }    

        public DbRiverRaceLog() { }

        public DbRiverRaceLog(Guid guid, DateTime timestamp, int seasonid, int sectionid, PeriodType type)
        {
            Guid = guid;
            TimeStamp = timestamp;
            SeasonId = seasonid;
            SectionId = sectionid;
            Type = type;
        }
    }
}
