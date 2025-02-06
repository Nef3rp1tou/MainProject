using CasinoApi.DTOs;
using CasinoApi.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CasinoApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalanceAsync([FromQuery] GetBalanceRequestDto request)
        {
            var result = await _userService.GetBalanceAsync(request);
            return Ok(result);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetPlayerInfoAsync([FromQuery] Guid token)
        {
            var result = await _userService.GetPlayerInfoAsync(token);
            return Ok(result);
        }
    }
}
