namespace ClashRoyaleApi.Models
{
    public class RiverRaceLogStandings
    {
        public int Rank { get; set; }   

        public int TrophyChange { get; set; }

        public RiverRaceClan Clan { get; set; } = new RiverRaceClan();

    }
}
