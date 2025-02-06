using CasinoApi.DTOs;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IServices;
using CasinoApi.Utilities;
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

        [HttpPost("balance")]
        public async Task<IActionResult> GetBalanceAsync([FromBody] GetBalanceRequestDto request)
        {
            var result = await _userService.GetBalanceAsync(request);
            return Ok(new CustomResponse(CustomStatusCode.Success, result));
        }

        [HttpPost("info")]
        public async Task<IActionResult> GetPlayerInfoAsync([FromBody] UserInfoRequestDto info)
        {
            var result = await _userService.GetPlayerInfoAsync(info.Token);
            return Ok(new CustomResponse(CustomStatusCode.Success, result));
        }
    }
}
