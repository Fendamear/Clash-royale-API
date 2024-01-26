using System.Diagnostics;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Logic.Logging;
using ClashRoyaleApi.Logic.MailHandler;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using Quartz;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.EventScheduler
{
    public class RiverRaceScheduler : IJob
    {
        private readonly ICurrentRiverRace _riverrace;
        private readonly ICrLogger _logger;
        private readonly IMailHandler _mailHandler;

        public RiverRaceScheduler(ICurrentRiverRace riverRace, ICrLogger logger, IMailHandler handler)
        {
            _riverrace = riverRace;
            _logger = logger;
            _mailHandler = handler;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Debug.WriteLine("task reached");
                SchedulerTime time = (SchedulerTime)context.MergedJobDataMap.Values.First();
                Response response = await _riverrace.CurrentRiverRaceScheduler(time);
                _logger.CurrentRiverRaceLog(response);
                _mailHandler.SendEmail(response);
            }
            catch (Exception ex)
            {

                
            }
            
        }
    }
}
