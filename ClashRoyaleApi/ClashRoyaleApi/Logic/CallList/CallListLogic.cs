using ClashRoyaleApi.Data;
using ClashRoyaleApi.DTOs.CallList;
using ClashRoyaleApi.Models.DbModels;

namespace ClashRoyaleApi.Logic.CallList
{
    public class CallListLogic : ICallList
    {
        private readonly DataContext _context;

        public CallListLogic(DataContext context) 
        { 
            _context = context;
        }

        public List<CallListDTO> GetCallLists()
        {
            List<CallListDTO> response = new List<CallListDTO>();

            List<DbClanMembers> members =  _context.DbClanMembers.Where(x => x.IsInClan).ToList();
            List<DbCallList> callLists = _context.CallList.ToList();

            List<DbClanMembers> coLeaders = members.Where(x => x.Role == "coLeader" || x.Role == "leader").ToList();
            
            foreach (DbClanMembers coLeader in coLeaders)
            {
                members.Remove(coLeader);

                CallListDTO callList = new CallListDTO();
                callList.CoLeader = coLeader.Name;
                callList.CoLeaderTag = coLeader.ClanTag;
                callList.Members = ConvertToCallListMemberDTO(callLists.Where(x => x.ColeaderClanTag == coLeader.ClanTag).ToList(), members);
                response.Add(callList);
            }

            CallListDTO unassigned = new CallListDTO();
            unassigned.CoLeader = "Unassigned";
            unassigned.CoLeaderTag = "Unassigned";
            unassigned.Members = ConvertToCallListMemberDTO(members);

            response.Insert(0, unassigned);
            return response;
        }

        public void AddMemberToCoLeader(string coleader, string member)
        {
            
        }

        public void RemoveMemberFromCoLeader(string coleader, string member)
        {
            throw new NotImplementedException();
        }

        public void ReplaceCoLeader(string coleaderOld, string coleaderNew)
        {
            throw new NotImplementedException();
        }

        public void UpdateMembers(List<CallListDTO> callList)
        {
            _context.CallList.RemoveRange(_context.CallList.ToList());

            foreach (CallListDTO entry in callList)
            {
                if (entry.CoLeaderTag == "Unassigned") continue;

                foreach (CallListMemberDTO member in entry.Members)
                {
                    _context.Add(new DbCallList(entry.CoLeader, entry.CoLeaderTag, member.ClanName, member.ClanTag));
                }
            }
            _context.SaveChanges();
        }

        public void init(string path)
        {
            throw new NotImplementedException();
        }

        private List<CallListMemberDTO> ConvertToCallListMemberDTO(List<DbCallList> members, List<DbClanMembers> clanMembers)
        {
            List<CallListMemberDTO> callListMemberDTOs = new List<CallListMemberDTO>();

            foreach (DbCallList callListMember in members)
            {
                callListMemberDTOs.Add(new CallListMemberDTO(callListMember.MemberTag, callListMember.Member));
                clanMembers.Remove(clanMembers.Where(x => x.ClanTag == callListMember.MemberTag).FirstOrDefault());
            }
            return callListMemberDTOs;  
        }

        private List<CallListMemberDTO> ConvertToCallListMemberDTO(List<DbClanMembers> members)
        {
            List<CallListMemberDTO> memberDTOs = new List<CallListMemberDTO>();

            foreach (DbClanMembers member in members)
            {
                memberDTOs.Add(new CallListMemberDTO(member.ClanTag, member.Name));
            }
            return memberDTOs;
        }

    }
}
