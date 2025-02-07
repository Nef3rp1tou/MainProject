using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using MvcProject.Utilities;

namespace MvcProject.Controllers;

[Authorize(Roles = "Player")]

public class PaymentController : Controller
{
    private readonly IBankingApiService _bankingApiService;

    public PaymentController(IBankingApiService bankingApiService)
    {
        _bankingApiService = bankingApiService;
    }

    [HttpGet("payment/dummy")]
    public IActionResult Dummy(string transactionId, decimal amount)
    {
        ViewData["TransactionId"] = transactionId;
        ViewData["Amount"] = amount;
        return View();
    }

    [HttpPost("payment/senddepositfinish")]
    public async Task<IActionResult> SendDepositFinish([FromBody] DepositFinishRequestDto request)
    {
        var response = await _bankingApiService.SendDepositFinishRequestAsync(request.TransactionId, request.Amount);

        return Ok(new CustomResponse(CustomStatusCode.Success, response, "Deposit finish processed successfully"));

    }
}
