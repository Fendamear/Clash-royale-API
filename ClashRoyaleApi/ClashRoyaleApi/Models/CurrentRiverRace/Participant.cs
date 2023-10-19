namespace ClashRoyaleApi.Models.CurrentRiverRace
{
    public class Participant
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public int Fame { get; set; }
        public int RepairPoints { get; set; }
        public int BoatAttacks { get; set; }
        public int DecksUsed { get; set; }
        public int DecksUsedToday { get; set; }
    }
}
