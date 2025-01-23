using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using MvcProject.Services;
using System.Security.Claims;


[Authorize]
public class TransactionsController : Controller
{
    private readonly IDepositWithdrawRequestsService _requestService;
    private readonly IBankingApiService _bankingApiService;

    public TransactionsController(IDepositWithdrawRequestsService requestService, IBankingApiService bankingApiService)
    {
        _requestService = requestService;
        _bankingApiService = bankingApiService;
    }

    // Deposit Page
    [HttpGet]
    public IActionResult Deposit()
    {
        return View();
    }

    // Handle Deposit Submission


    [HttpPost]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequestDto model)
    {
        decimal amount = model.Amount;
        if (amount <= 0)
        {
            return Json(new { success = false, message = "Amount must be greater than zero." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var transactionId = Guid.NewGuid();

            // Create the deposit request in the database
            await _requestService.CreateRequestAsync(transactionId, userId, TransactionType.Deposit, amount);

            // Send the deposit request to the Banking API and get the PaymentUrl
            var response = _bankingApiService.SendDepositRequestAsync(transactionId, amount);

            // Redirect the user to the PaymentUrl
            return Json(new { success = true, redirectUrl = response.Result.PaymentUrl });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Withdraw([FromBody] TransactionRequestDto model)
    {
        if (model.Amount <= 0)
        {
            return Json(new { success = false, message = "Amount must be greater than zero." });
        }
        if (string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.CardNumber))
        {
            return Json(new { success = false, message = "Full name and card number are required." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            // Create a withdrawal request with a Pending status
            var transactionId = Guid.NewGuid();
            await _requestService.CreateRequestAsync(transactionId, userId, TransactionType.Withdraw, model.Amount);

            TempData["FullName"] = model.FullName;
            TempData["CardNumber"] = model.CardNumber;

            return Json(new { success = true, message = "Withdrawal request submitted successfully and is awaiting admin approval." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet]
    public IActionResult Withdraw()
    {
        return View();
    }




    //// Transaction History Page
    //[HttpGet]
    //public async Task<IActionResult> TransactionHistory()
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    try
    //    {
    //        var transactions = await _requestService.GetRequestsByUserIdAsync(userId);
    //        return View(transactions);
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["ErrorMessage"] = $"An error occurred while fetching your transaction history: {ex.Message}";
    //        return RedirectToAction("Index", "Home");
    //    }
    //}
}
