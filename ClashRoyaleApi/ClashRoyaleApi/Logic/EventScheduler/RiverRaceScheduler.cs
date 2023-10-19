using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Logic.Logging;
using ClashRoyaleApi.Logic.Logging.LoggingModels;
using Quartz;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.EventScheduler
{
    public class RiverRaceScheduler : IJob
    {
        private readonly ICurrentRiverRace _riverrace;
        private readonly ICrLogger _logger;

        public RiverRaceScheduler(ICurrentRiverRace riverRace, ICrLogger logger)
        {
            _riverrace = riverRace;
            _logger = logger;

        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                SchedulerTime time = (SchedulerTime)context.MergedJobDataMap.Values.First();
                CurrentRiverRaceLog log = _riverrace.CurrentRiverRaceScheduler(time);
                _logger.CurrentRiverRaceLog(log);
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
