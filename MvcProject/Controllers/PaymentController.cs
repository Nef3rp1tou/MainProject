using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Interfaces.IServices;

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
    public async Task<IActionResult> SendDepositFinish([FromBody] DepositRequestDto request)
    {
        try
        {
            var response = await _bankingApiService.SendDepositFinishRequestAsync(request.TransactionId, request.Amount);

            return Ok(new { status = response.Status, message = "Deposit finish processed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}
