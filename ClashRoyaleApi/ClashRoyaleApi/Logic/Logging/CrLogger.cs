using ClashRoyaleApi.Data;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;

namespace ClashRoyaleApi.Logic.Logging
{
    public class CrLogger : ICrLogger
    {
        private readonly DataContext _dataContext;    

        public CrLogger(DataContext context) 
        { 
            _dataContext = context;      
        }

        public void CurrentRiverRaceLog(Response response)
        {
            _dataContext.CurrentRiverRaceLogs.Add(response.log);
            _dataContext.SaveChanges();
        }

    }
}
