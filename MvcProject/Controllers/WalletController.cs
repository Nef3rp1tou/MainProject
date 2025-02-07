using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using MvcProject.Utilities;
using System.Security.Claims;

namespace MvcProject.Controllers;
[Authorize]
[Route("[controller]")]
public class WalletController : Controller
{
    private IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }


    [HttpGet]
    public async Task<IActionResult> GetBalance()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wallet = await _walletService.GetWalletByUserIdAsync(userId ?? string.Empty);
        return Ok(new CustomResponse(CustomStatusCode.Success, wallet));
    }

}
