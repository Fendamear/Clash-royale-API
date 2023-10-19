namespace ClashRoyaleApi.Models.CurrentRiverRace
{
    public class PeriodLogItem
    {
        public Clan Clan { get; set; }
        public int PointsEarned { get; set; }
        public int ProgressStartOfDay { get; set; }
        public int ProgressEndOfDay { get; set; }
        public int EndOfDayRank { get; set; }
        public int ProgressEarned { get; set; }
        public int NumOfDefensesRemaining { get; set; }
        public int ProgressEarnedFromDefenses { get; set; }
    }
}
