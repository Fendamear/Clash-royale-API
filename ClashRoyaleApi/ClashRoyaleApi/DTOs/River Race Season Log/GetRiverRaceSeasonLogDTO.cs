using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.DTOs.River_Race_Season_Log
{
    public class GetRiverRaceSeasonLogDTO
    {
        public string Guid { get; set; }

        public DateTime TimeStamp { get; set; }

        public int SeasonId { get; set; }

        public string SectionId { get; set; }

        public PeriodType type { get; set; }

    }
}
