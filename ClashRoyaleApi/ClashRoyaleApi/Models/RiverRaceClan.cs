namespace ClashRoyaleApi.Models
{
    public class RiverRaceClan
    {
        public string? Tag { get; set; }

        public string? Name { get; set; }    

        public int BadgeId { get; set; }

        public int Fame { get; set; }

        public int RepairPoints { get; set; }

        public string? FinishTime { get; set; }

        public List<RiverRaceParticipant> Participants { get; set; } = new List<RiverRaceParticipant>();

    }
}
