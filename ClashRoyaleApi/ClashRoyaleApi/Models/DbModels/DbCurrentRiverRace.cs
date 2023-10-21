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

        public DbCurrentRiverRace()
        {

        }

        public DbCurrentRiverRace(Guid guid, int seasonId, int sectionId, int dayId, string? seasonSectionDay, string? tag, string? name, int fame, int decksUsedToday, int decksNotUsed, SchedulerTime schedule)
        {
            Guid = guid;
            SeasonId = seasonId;
            SectionId = sectionId;
            DayId = dayId;
            SeasonSectionDay = seasonSectionDay;
            Tag = tag;
            Name = name;
            Fame = fame;
            DecksUsedToday = decksUsedToday;
            DecksNotUsed = decksNotUsed;
            Schedule = schedule;
        }
    }
}
