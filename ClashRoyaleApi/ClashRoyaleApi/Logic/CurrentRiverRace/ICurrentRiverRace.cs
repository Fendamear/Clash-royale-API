using ClashRoyaleApi.DTOs.River_Race_Season_Log;
using ClashRoyaleApi.Models.CurrentRiverRace;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public interface ICurrentRiverRace
    {
        Task<Root> GetCurrentRiverRace();

        Task<Response> CurrentRiverRaceScheduler(SchedulerTime time);

        Task PostRiverRaceLog(PostRiverRaceLogDTO post);

        Task<List<GetRiverRaceSeasonLogDTO>> GetRiverRaceSeasonLog();

        Task<bool> DeleteRiverRaceLog(int seasonId, int sectionId);

        int GetSeasonId(int sectionId, PeriodType type);
    }
}
