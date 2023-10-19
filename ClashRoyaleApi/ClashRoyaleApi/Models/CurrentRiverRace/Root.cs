namespace ClashRoyaleApi.Models.CurrentRiverRace
{
    public class Root
    {
        public string state { get; set; }
        public Clan clan { get; set; }
        public List<Clan> clans { get; set; }
        public int sectionIndex { get; set; }
        public int periodIndex { get; set; }
        public string periodType { get; set; }
        public List<PeriodLog> periodLogs { get; set; }

    }
}
