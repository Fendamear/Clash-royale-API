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
            await _ClanMemberLogic.GetClanInfo();
        }
    }
}
