using ClashRoyaleApi.DTOs.MailSubscriptions;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Logic.MailHandler;
using ClashRoyaleApi.Logic.MailHandler.MailSubscription;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using ClashRoyaleApi.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public ActionResult<List<MailSubscriptionsDTO>> GetMailSubscriptions()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Ok(_mailSubscription.GetMailSubscriptions(UtilityClass.GetValueFromToken(identity, JWTToken.ClanTag)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[Controller]/UpdateMailSubscriptions")]
        [Authorize]
        public ActionResult UpdateMailSubscription([FromBody] List<MailSubscriptionsDTO> dto)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                _mailSubscription.UpdateMailSubscriptions(dto, UtilityClass.GetValueFromToken(identity, JWTToken.ClanTag));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
