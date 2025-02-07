using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using MvcProject.Utilities;
using System.Security.Claims;

namespace MvcProject.Controllers
{
    [Authorize(Roles = "Player")]
    public class AuthController : Controller
    {
        private readonly IUserTokenService _userTokenService;

        public AuthController(IUserTokenService userTokenService)
        {
            _userTokenService = userTokenService;
        }

        // Generate Public Token
        [HttpPost]
        public async Task<IActionResult> Generate()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException(CustomStatusCode.UserNotFound, "User not found.");
            }

            var publicToken = await _userTokenService.GeneratePublicToken(userId);
            return Ok(new CustomResponse(CustomStatusCode.Success, publicToken));
        }


        [HttpGet]
        public IActionResult GenerateToken()
        {
            return View();
        }
    }
}
