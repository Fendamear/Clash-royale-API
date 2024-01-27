using ClashRoyaleApi.DTOs.Authentication;
using ClashRoyaleApi.DTOs.Authentication.Register;
using ClashRoyaleApi.Logic.Authentication;
using ClashRoyaleApi.Models;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz.Core;
using System.Security.Claims;

namespace ClashRoyaleApi.Controllers
{
    [ApiController()]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationLogic _authenticationLogic;

        public AuthenticationController(IAuthenticationLogic authenticationLogic)
        {
            _authenticationLogic = authenticationLogic;
        }

        [HttpPost("[controller]/registerUser")]
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
         
        [HttpPost("[controller]/registerwithclantag")]
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

        [HttpGet("[controller]/GetClanTags")]
        public ActionResult<List<ClanTagNameDTO>> GetClanTags()
        {
            try
            {
                return Ok(_authenticationLogic.GetAllClanTagsWithNameFromDbClanMemberDb());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/[Controller]/login", Name = "Login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO dto)
        {
            TokenDTO token = new TokenDTO();

            try
            {
                token = await _authenticationLogic.GenerateToken(dto);
                if (token.Token == "") return BadRequest();
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/[Controller]/Authorize")]
        [Authorize]
        public ActionResult TestAuthorization()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return Ok(UtilityClass.GetValueFromToken(identity, EnumClass.JWTToken.ID));
        }

        [HttpGet("/[Controller]/Authorize/Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult TestAuthorizationAdmin()
        {
            return Ok();
        }

        [HttpGet("/[Controller]/Authorize/AdminAndCoLeader")]
        [Authorize(Roles = "Admin, CoLeader")]
        public ActionResult TestAuthorizationAdminAndCo()
        {
            return Ok();
        }
    }
}
