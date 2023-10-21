using ClashRoyaleApi.DTOs.Clan;
using ClashRoyaleApi.Logic.ClanMembers;
using Microsoft.AspNetCore.Mvc;

namespace ClashRoyaleApi.Controllers
{
    public class ClanMemberController : Controller
    {
        private readonly IClanMemberLogic _ClanMemberLogic;

        public ClanMemberController(IClanMemberLogic iclanMemberLogic)
        {
            _ClanMemberLogic = iclanMemberLogic;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet("/ClanMembers")]
        public async Task test()
        {
            await _ClanMemberLogic.RetrieveClanInfoScheduler();
        }

        [HttpGet("/GetClanMembers")]
        public async Task<ActionResult<List<GetClanMemberInfoDTO>>> GetClanMemberInfo()
        {
            try
            {
                return Ok(await _ClanMemberLogic.GetClanMemberInfo());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/GetClanMemberLog")]
        public async Task<ActionResult<List<GetClanMemberLogDTO>>> GetClanMemberLog()
        {
            try
            {
                return Ok(await _ClanMemberLogic.GetClanMemberLog());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/DeleteClanMemberLog")]
        public async Task<ActionResult<List<GetClanMemberLogDTO>>> DeleteClanMemberLog(Guid guid)
        {
            try
            {
                await _ClanMemberLogic.DeleteRiverRaceLog(guid);
                return Ok("log deleted succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
