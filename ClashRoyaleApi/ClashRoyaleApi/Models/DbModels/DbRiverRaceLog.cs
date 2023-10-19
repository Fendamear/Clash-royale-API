using System.ComponentModel.DataAnnotations;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Models.DbModels
{
    public class DbRiverRaceLog
    {
        [Key]
        public Guid Guid { get; set; }

        public DateTime timeStamp { get; set; }

        public int SeasonId { get; set; }

        public int SectionId { get; set; }

        public PeriodType type { get; set; }    

    }
}
