using System.ComponentModel.DataAnnotations;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Models.DbModels
{
    public class DbCurrentRiverRace
    {
        [Key]
        public Guid Guid { get; set; }

        public int SeasonId { get; set; }

        public int SectionId { get; set; }  

        public int DayId { get; set; }

        public string? SeasonSectionDay { get; set; }

        public string? Tag { get; set; }

        public string? Name { get; set; }

        public int Fame { get; set; }

        public int DecksUsedToday { get; set; }

        public int DecksNotUsed { get; set; }

        public SchedulerTime Schedule { get; set; }
    }
}
