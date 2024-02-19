using AutoMapper.Configuration.Conventions;

namespace ClashRoyaleApi.DTOs.Current_River_Race.Homepage.AllTime
{
    public class AllTimeDataDTO
    {
        public LowestHighestDTO HighestDecksNotUsed { get; set; }

        public LowestHighestDTO LowestDecksNotUsed { get; set; }

        public LowestHighestDTO HighestFameScore { get; set; }

        public LowestHighestDTO LowestFameScore { get; set; }

        public int HighestClanWarScoreWeek { get; set; }

        public int HighestClanWarScoreDay { get; set; } 

        public int TotalUnusedDecks { get; set; }
    }
}
