using ClashRoyaleApi.Data;
using ClashRoyaleApi.Logic.Logging.LoggingModels;

namespace ClashRoyaleApi.Logic.Logging
{
    public class CrLogger : ICrLogger
    {
        private readonly DataContext _dataContext;    

        public CrLogger(DataContext context) 
        { 
            _dataContext = context;      
        }

        public void CurrentRiverRaceLog(CurrentRiverRaceLog log)
        {
            _dataContext.CurrentRiverRaceLogs.Add(log);
            _dataContext.SaveChanges();
        }

    }
}
