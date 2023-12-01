namespace ClashRoyaleApi.DTOs.CallList
{
    public class CallListDTO
    {
        public string CoLeader { get; set; }

        public string CoLeaderTag { get; set; }

        public List<CallListMemberDTO> Members { get; set; }

        public CallListDTO() { }
    }
}
