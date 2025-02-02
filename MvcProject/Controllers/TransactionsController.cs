using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Interfaces.IServices;
using System.Security.Claims;

[Authorize]
public class TransactionsController : Controller
{
    private readonly ITransactionsHandlerService _transactionsHandlerService;
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionsHandlerService transactionsHandlerService, ITransactionService transactionService)
    {
        _transactionsHandlerService = transactionsHandlerService;
        _transactionService = transactionService;
    }

    [HttpGet]
    public IActionResult Deposit()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequestDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Json(new { success = false, message = "User is not authenticated." });

        var result = await _transactionsHandlerService.HandleDepositAsync(userId, model.Amount);
        return Json(new { statusCode = result.StatusCode, success = result.IsSuccess, message = result.Message, redirectUrl = result.Data });
    }

    [HttpPost]
    public async Task<IActionResult> Withdraw([FromBody] TransactionRequestDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Json(new { statusCode = StatusCodes.Status401Unauthorized, success = false, message = "User is not authenticated." });

        var result = await _transactionsHandlerService.HandleWithdrawAsync(userId, model.Amount);
        return Json(new { statusCode = result.StatusCode, success = result.IsSuccess, message = result.Message, Data = result.Data});
    }

    [HttpGet]
    public IActionResult Withdraw()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var transactions = await _transactionService.GetTransactionsByUserIdAsync(userId);
        return View(transactions);
    }
}
