using ClashRoyaleApi.Logic.RiverRace;
using ClashRoyaleApi.Models.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace ClashRoyaleApi.Controllers
{
    public class RiverRaceController : Controller
    {
        private readonly IRiverRaceLogic _riverRaceLogic;

        public RiverRaceController(IRiverRaceLogic riverRaceLogic)
        {
            _riverRaceLogic = riverRaceLogic;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
