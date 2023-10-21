using ClashRoyaleApi.DTOs.River_Race_Season_Log;
using ClashRoyaleApi.Logic.Logging.LoggingModels;
using ClashRoyaleApi.Models.CurrentRiverRace;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public interface ICurrentRiverRace
    {
        Task<Root> GetCurrentRiverRace();

        Task<CurrentRiverRaceLog> CurrentRiverRaceScheduler(SchedulerTime time);

        Task PostRiverRaceLog(PostRiverRaceLogDTO post);

        Task<List<GetRiverRaceSeasonLogDTO>> GetRiverRaceSeasonLog();

        Task<bool> DeleteRiverRaceLog(int seasonId, int sectionId);

        int GetSeasonId(int sectionId, PeriodType type);
    }
}
