using ClashRoyaleApi.DTOs.Clan;

namespace ClashRoyaleApi.Logic.ClanMembers
{
    public interface IClanMemberLogic
    {
        Task RetrieveClanInfoScheduler();

        Task DeleteRiverRaceLog(Guid guid);

        Task<List<GetClanMemberLogDTO>> GetClanMemberLog();

        Task<List<GetClanMemberInfoDTO>> GetClanMemberInfo();
    }
}
