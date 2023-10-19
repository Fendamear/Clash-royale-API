using ClashRoyaleApi.Logic.CurrentRiverRace;
using Microsoft.AspNetCore.Mvc;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Controllers
{
    public class CurrentRiverRaceController : Controller
    {
        private readonly ICurrentRiverRace _currentRiverRace;
        public CurrentRiverRaceController(ICurrentRiverRace currentRiverRace) 
        { 
            _currentRiverRace = currentRiverRace;
        }


        [HttpGet("/currentriverrace")]
        public ActionResult GetCurrentRiverRace(SchedulerTime time)
        {
            try
            {
                //return Ok(_currentRiverRace.GetCurrentRiverRace());
                return Ok(_currentRiverRace.CurrentRiverRaceScheduler(time));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
