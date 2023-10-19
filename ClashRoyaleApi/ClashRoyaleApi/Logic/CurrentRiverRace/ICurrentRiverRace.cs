using ClashRoyaleApi.Models.CurrentRiverRace;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public interface ICurrentRiverRace
    {
        Task<Root> GetCurrentRiverRace();
    }
}
