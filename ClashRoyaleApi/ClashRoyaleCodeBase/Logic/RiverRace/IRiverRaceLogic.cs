using ClashRoyaleCodeBase.Models.JsonModels;

namespace ClashRoyaleCodeBase.Logic.RiverRace
{
    public interface IRiverRaceLogic
    {
        Task<Root> GetRiverRaceLog(int limit);

    }
}
