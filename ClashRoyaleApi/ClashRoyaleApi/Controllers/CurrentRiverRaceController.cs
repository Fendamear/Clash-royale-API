using ClashRoyaleApi.DTOs.Current_River_Race;
using ClashRoyaleApi.DTOs.River_Race_Season_Log;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("CurrentRiverRace/currentriverrace")]
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

        [HttpGet("CurrentRiverRace/GetCurrentriverrace")]
        public ActionResult GetCurrentRiverRace(int seasonId, int sectionId, int dayId)
        {
            try
            {
                //return Ok(_currentRiverRace.GetCurrentRiverRace());
                return Ok(_currentRiverRace.GetCurrentRiverRace(seasonId, sectionId, dayId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }


        [HttpGet("CurrentRiverRace/GetRiverRaceLog")]
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

        [HttpPost("CurrentRiverRace/PostRiverRaceLog")]
        [Authorize(Roles = "Admin")]
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

        [HttpDelete("CurrentRiverRace/DeleteRiverRaceLog")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteRiverRaceSeasonLog(string id)
        {
            try
            {
                if (!await _currentRiverRace.DeleteRiverRaceLog(id))
                    return BadRequest("River Race Log does not exist");
                return Ok("River Race log succesfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/CurrentRiverRace/GraphData")]
        public ActionResult<List<GetGraphDataDTO>> GetRiverRaceData(int seasonId, int sectionId, bool notUsed)
        {
            try
            {
                return Ok(_currentRiverRace.GetGraphData(seasonId, sectionId, notUsed));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
