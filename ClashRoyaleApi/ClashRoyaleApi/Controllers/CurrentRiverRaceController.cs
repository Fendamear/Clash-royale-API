using ClashRoyaleApi.Logic.CurrentRiverRace;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult GetCurrentRiverRace()
        {
            try
            {
               return Ok(_currentRiverRace.GetCurrentRiverRace());
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
