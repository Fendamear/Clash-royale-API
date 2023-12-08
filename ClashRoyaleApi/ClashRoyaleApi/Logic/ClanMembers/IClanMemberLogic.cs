using ClashRoyaleApi.DTOs.Clan;
using ClashRoyaleApi.Models.DbModels;
using System.Net.Http;

namespace ClashRoyaleApi.Logic.ClanMembers
{
    public interface IClanMemberLogic
    {
        Task<List<DbClanMembers>> RetrieveClanInfoScheduler();

        Task DeleteRiverRaceLog(Guid guid);

        Task<List<GetClanMemberLogDTO>> GetClanMemberLog();

        Task<List<GetClanMemberInfoDTO>> GetClanMemberInfo();

        GetLatestLogTimeDTO GetLatestLogTime();
               
    }
}
