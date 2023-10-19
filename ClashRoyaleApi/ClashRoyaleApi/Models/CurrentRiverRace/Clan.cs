namespace ClashRoyaleApi.Models.CurrentRiverRace
{
    public class Clan
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public int BadgeId { get; set; }
        public int Fame { get; set; }
        public int RepairPoints { get; set; }
        public List<Participant> Participants { get; set; }
        public int PeriodPoints { get; set; }
        public int ClanScore { get; set; }
    }
}
