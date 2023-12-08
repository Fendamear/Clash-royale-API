namespace ClashRoyaleApi.DTOs.Clan
{
    public class GetLatestLogTimeDTO
    {
        public DateTime Time { get; set; }

        public GetLatestLogTimeDTO(DateTime time)
        {
            Time = time;
        }
    }
}
