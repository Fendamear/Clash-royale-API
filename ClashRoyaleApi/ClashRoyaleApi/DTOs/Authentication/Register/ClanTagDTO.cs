namespace ClashRoyaleApi.DTOs.Authentication.Register
{
    public class ClanTagDTO
    {
        public string? Tag { get; set; }


        public ClanTagDTO(string tag)
        {
            Tag = tag;
        }
    }
}
