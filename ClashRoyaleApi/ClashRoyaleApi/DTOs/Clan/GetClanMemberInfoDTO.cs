namespace ClashRoyaleApi.DTOs.Clan
{
    public class GetClanMemberInfoDTO
    {
        public string Tag { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public DateTime LastSeen { get; set; }

        public bool IsActive { get; set; }

        public bool IsInClan { get; set; }

    }
}
