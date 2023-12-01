using ClashRoyaleApi.DTOs.CallList;
using ClashRoyaleApi.Logic.CallList;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ClashRoyaleApi.Controllers
{
    [ApiController]
    public class CallListController : Controller
    {
        private readonly ICallList _callList;

        public CallListController(ICallList callList) 
        { 
            _callList = callList;
        }

        [HttpGet("/CallList")]
        public ActionResult<List<CallListDTO>> getCallList()
        {
            try
            {
                return Ok(_callList.GetCallLists());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpPost("/CallList")]
        public ActionResult SaveCallList(List<CallListDTO> callList)
        {
            try
            {
                _callList.UpdateMembers(callList);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
