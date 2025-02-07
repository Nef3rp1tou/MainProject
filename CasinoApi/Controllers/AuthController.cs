using CasinoApi.DTOs;
using CasinoApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IServices;


namespace CasinoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticatorController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthenticatorController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("auth")]
    public async Task<IActionResult> ValidatePublicToken([FromBody] TokenRequestDto request)
    { 
       
        var privateToken = await _tokenService.CreatePrivateTokenAsync(request);

        return Ok(new CustomResponse(CustomStatusCode.Success, new TokenResponseDto(privateToken)));
    }
}










