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
        public async Task<ActionResult> DeleteRiverRaceSeasonLog(int seasonId, int sectionId)
        {
            try
            {
                if (!await _currentRiverRace.DeleteRiverRaceLog(seasonId, sectionId))
                    return BadRequest("River Race Log does not exist");
                return Ok("River Race log succesfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/test")]
        public ActionResult TestMethod(string datetime)
        {
            try
            {
                DateTime response;
                if (!DateTime.TryParse(datetime, out response))
                {
                    throw new InvalidDataException("Date String is invalid");
                }
                DateTime utcTime5MinutesBefore = TimeZoneInfo.ConvertTimeToUtc(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 58, 0));
                return Ok(utcTime5MinutesBefore);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
