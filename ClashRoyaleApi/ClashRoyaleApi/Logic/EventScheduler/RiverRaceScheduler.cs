using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Logic.Logging;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
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

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                SchedulerTime time = (SchedulerTime)context.MergedJobDataMap.Values.First();
                Response response = await _riverrace.CurrentRiverRaceScheduler(time);
                _logger.CurrentRiverRaceLog(response);
                await Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
