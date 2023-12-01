using ClashRoyaleApi.DTOs.MailSubscriptions;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Logic.MailHandler;
using ClashRoyaleApi.Logic.MailHandler.MailSubscription;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Controllers
{

    public class MailController : Controller
    {
        private readonly IMailHandler _mailHandler;
        private readonly ICurrentRiverRace _currentRiverRace;
        private readonly IMailSubscription _mailSubscription;

        public MailController(IMailHandler mailHandler, ICurrentRiverRace currentRiverRace, IMailSubscription mailSubscription) 
        { 
            _mailHandler = mailHandler;
            _currentRiverRace = currentRiverRace;
            _mailSubscription = mailSubscription;
        }

        [HttpGet("[Controller]/TestMail")]
        public async Task<IActionResult> TestMail(MailType type, SchedulerTime time, Status status) 
        {
            try
            {
                _mailHandler.SendEmail(await _currentRiverRace.CurrentRiverRaceScheduler(time));
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        
        }

        [HttpGet("[Controller]/GetMailSubscription")]
        public ActionResult<List<MailSubscriptionsDTO>> GetMailSubscriptions()
        {
            try
            {
                string clantag = "#LY0UQ9R";
                return Ok(_mailSubscription.GetMailSubscriptions(clantag));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[Controller]/UpdateMailSubscriptions")]
        public ActionResult UpdateMailSubscription([FromBody] List<MailSubscriptionsDTO> dto)
        {
            try
            {
                string clantag = "#LY0UQ9R";
                _mailSubscription.UpdateMailSubscriptions(dto, clantag);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            
    }
}
