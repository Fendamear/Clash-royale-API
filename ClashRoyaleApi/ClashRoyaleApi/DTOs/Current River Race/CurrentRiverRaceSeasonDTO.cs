namespace ClashRoyaleApi.DTOs.Current_River_Race
{
    public class CurrentRiverRaceSeasonDTO
    {
        public string Name { get; set; }

        public string Tag { get; set; }

        public string DateIdentifier { get; set; }

        public DateTime Time { get; set; }

        public int Fame { get; set; }

        public int DecksUsed { get; set; }

        public int DecksNotUsed { get; set; }

        public List<CurrentRiverRaceSectionDTO> subRows { get; set; }

    }
}
