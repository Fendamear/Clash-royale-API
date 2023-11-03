namespace ClashRoyaleApi.DTOs.Authentication
{
    public class ClanTagNameDTO
    {
        public string ClanTag { get; set; } 

        public string Name { get; set; }

        public ClanTagNameDTO(string clantag, string name) 
        { 
            ClanTag = clantag;
            Name = name;
        }
    }
}
