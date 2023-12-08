namespace ClashRoyaleApi.DTOs.Current_River_Race
{
    public class CurrentRiverRaceDayDTO
    {

        public string DateIdentifier { get; set; }

        public DateTime Time { get; set; }

        public int Fame { get; set; }

        public int DecksUsed { get; set; }

        public int DecksNotUsed { get; set; }

    }
}
