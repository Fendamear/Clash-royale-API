using ClashRoyaleApi.DTOs.CallList;

namespace ClashRoyaleApi.Logic.CallList
{
    public interface ICallList
    {
        void AddMemberToCoLeader(string coleader, string member);

        void RemoveMemberFromCoLeader(string coleader, string member);

        void ReplaceCoLeader(string coleaderOld, string coleaderNew);

        void UpdateMembers(List<CallListDTO> callList);

        List<CallListDTO> GetCallLists();

        void init(string path);
    }
}
