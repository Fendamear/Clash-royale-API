using ClashRoyaleApi.DTOs.Authentication.Register;
using ClashRoyaleApi.Logic.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClashRoyaleApi.Controllers
{
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationLogic _authenticationLogic;

        public AuthenticationController(IAuthenticationLogic authenticationLogic)
        {
            _authenticationLogic = authenticationLogic;
        }

        [HttpPost("/registeruser")]
        public async Task<ActionResult> RegisterUser(CreateUserDTO dto)
        {
            try
            {
                await _authenticationLogic.RegisterUser(dto);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/registerwithclantag")]
        public ActionResult RegisterWithClanTag(string clantag)
        {
            try
            {
                _authenticationLogic.RegisterWithClanTag(clantag);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






    }
}
