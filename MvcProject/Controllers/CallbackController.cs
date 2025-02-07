using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    [ApiController]
    [Route("callback")]
    public class CallbackController : ControllerBase
    {
        private readonly ICallbackService _callbackService;

        public CallbackController(ICallbackService callbackService)
        {
            _callbackService = callbackService;
        }

        [HttpPost("handle")]
        public async Task<IActionResult> HandleCallback([FromBody] CallbackRequestModel callbackRequest)
        {
            var result = await _callbackService.ProcessCallbackAsync(callbackRequest, isWithdraw: false);
            return Ok(result);
        }

        [HttpPost("handlewithdraw")]
        public async Task<IActionResult> HandleWithdraw([FromBody] CallbackRequestModel callbackRequest)
        {
            var result = await _callbackService.ProcessCallbackAsync(callbackRequest, isWithdraw: true);
            return Ok(result);
        }
    }
}
