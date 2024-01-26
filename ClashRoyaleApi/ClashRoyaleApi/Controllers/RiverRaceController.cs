using ClashRoyaleApi.Logic.MailHandler;
using ClashRoyaleApi.Logic.RiverRace;
using ClashRoyaleApi.Models.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace ClashRoyaleApi.Controllers
{
    public class RiverRaceController : Controller
    {
        private readonly IRiverRaceLogic _riverRaceLogic;
        private readonly IMailHandler _mailHandler;

        public RiverRaceController(IRiverRaceLogic riverRaceLogic, IMailHandler mailHandler)
        {
            _riverRaceLogic = riverRaceLogic;
            _mailHandler = mailHandler;
        }

        [HttpGet("/RiverRace")] 
        public async Task<ActionResult<Root>> getRiverRace(int limit)
        {
            try
            {
               return await _riverRaceLogic.GetRiverRaceLog(limit);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/testendpoint")]
        public ActionResult result()
        {
            try
            {
                //_mailHandler.SendBuildMail();
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
