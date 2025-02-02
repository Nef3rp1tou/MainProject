using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Interfaces.IServices;
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBalance()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wallet = await _walletService.GetWalletByUserIdAsync(userId);
        return Json(new
        {
            balance = wallet.CurrentBalance,
            currency = wallet.Currency.ToString()
        });
    }

}
