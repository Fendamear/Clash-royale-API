namespace ClashRoyaleApi.DTOs.Current_River_Race.Homepage
{
    public class WeeklyDataDTO
    {
        public LowestHighestDTO Lowest { get; set; }

        public LowestHighestDTO Highest { get; set; }

        public List<GetGraphDataDTO> GraphData { get; set; }

    }
}
