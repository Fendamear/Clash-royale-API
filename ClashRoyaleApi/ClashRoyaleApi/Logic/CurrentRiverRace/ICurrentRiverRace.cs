using ClashRoyaleApi.Logic.Logging.LoggingModels;
using ClashRoyaleApi.Models.CurrentRiverRace;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public interface ICurrentRiverRace
    {
        Task<Root> GetCurrentRiverRace();

        CurrentRiverRaceLog CurrentRiverRaceScheduler(SchedulerTime time);
    }
}
