using ClashRoyaleApi.Logic.ClanMembers;
using ClashRoyaleApi.Logic.MailHandler;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using Quartz;

namespace ClashRoyaleApi.Logic.EventScheduler
{
    public class ClanMemberInfoScheduler : IJob
    {
        private readonly IClanMemberLogic _clanMemberLogic;
        private readonly IMailHandler _mailHandler;

        public ClanMemberInfoScheduler(IClanMemberLogic logic, IMailHandler mailHandler)
        {
            _clanMemberLogic = logic;
            _mailHandler = mailHandler;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _clanMemberLogic.RetrieveClanInfoScheduler();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.log.Status = Models.EnumClass.Status.FAILED;
                response.log.SchedulerTime = Models.EnumClass.SchedulerTime.CLANINFOSCHEDULE;
                response.Exception = ex;
                _mailHandler.SendEmail(response);
            }
        }
    }
}
