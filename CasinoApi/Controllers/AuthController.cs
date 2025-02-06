using CasinoApi.DTOs;
using CasinoApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IServices;


namespace CasinoApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidatePublicToken([FromBody] TokenRequestDto request)
    { 
       
        var privateToken = await _tokenService.CreatePrivateTokenAsync(request);

        return Ok(new CustomResponse(CustomStatusCode.Success, new TokenResponseDto(privateToken)));
    }
}










