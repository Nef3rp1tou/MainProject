using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using MvcProject.Utilities;
using System.Security.Claims;

namespace MvcProject.Controllers;

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
            throw new CustomException(CustomStatusCode.UserNotFound);

        var result = await _transactionsHandlerService.HandleDepositAsync(userId, model.Amount);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Withdraw([FromBody] TransactionRequestDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new CustomException(CustomStatusCode.UserNotFound);

        var result = await _transactionsHandlerService.HandleWithdrawAsync(userId, model.Amount);
        return Ok(result);
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
