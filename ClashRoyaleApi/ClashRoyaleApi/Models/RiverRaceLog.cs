namespace ClashRoyaleApi.Models
{
    public class RiverRaceLog
    {
        public int SeasonId { get; set; }

        public int SectionIndex { get; set; }

        public string? CreatedDate { get; set; }

        public List<RiverRaceLogStandings> Standings { get; set; } = new List<RiverRaceLogStandings>(); 
    }
}
