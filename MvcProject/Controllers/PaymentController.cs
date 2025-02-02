using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Interfaces.IServices;

namespace MvcProject.Controllers;
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

        return Ok(new { status = response.Status, message = "Deposit finish processed successfully" });
    }
}
