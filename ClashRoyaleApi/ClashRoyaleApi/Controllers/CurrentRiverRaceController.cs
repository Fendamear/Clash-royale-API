using ClashRoyaleApi.DTOs.River_Race_Season_Log;
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

        [HttpGet("/GetRiverRaceLog")]
        public async Task<ActionResult<List<GetRiverRaceSeasonLogDTO>>> GetRiverRaceSeasonLog()
        {
            try
            {
                return Ok(await _currentRiverRace.GetRiverRaceSeasonLog());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/PostRiverRaceLog")]
        public async Task<ActionResult> PostRiverRaceSeasonLog(PostRiverRaceLogDTO log)
        {
            try
            {
                await _currentRiverRace.PostRiverRaceLog(log);
                return Ok("Request Successfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/DeleteRiverRaceLog")]
        public async Task<ActionResult> DeleteRiverRaceSeasonLog(int seasonId, int sectionId)
        {
            try
            {
                if (!await _currentRiverRace.DeleteRiverRaceLog(seasonId, sectionId)) return BadRequest("River Race Log does not exist");
                return Ok("River Race log succesfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



















        public IActionResult Index()
        {
            return View();
        }
    }
}
