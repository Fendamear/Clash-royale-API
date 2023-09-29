using ClashRoyaleApi.Models.JsonModels;

namespace ClashRoyaleApi.Logic.RiverRace
{
    public interface IRiverRaceLogic
    {
        Task<Root> GetRiverRaceLog(int limit);

    }
}
