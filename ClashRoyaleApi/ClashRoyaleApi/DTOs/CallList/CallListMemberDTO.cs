namespace ClashRoyaleApi.DTOs.CallList
{
    public class CallListMemberDTO
    {
        public string ClanTag { get; set; } 

        public string ClanName { get; set;}

        public CallListMemberDTO(string clantag, string clanname)
        {
            ClanTag = clantag;
            ClanName = clanname;    
        }
    }
}
