using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using System.Security.Claims;


[Authorize]
public class TransactionsController : Controller
{
    private readonly IDepositWithdrawRequestsService _requestService;

    public TransactionsController(IDepositWithdrawRequestsService requestService)
    {
        _requestService = requestService;
    }

    // Deposit Page
    [HttpGet]
    public IActionResult Deposit()
    {
        return View();
    }

    // Handle Deposit Submission
    public class DepositRequestModel
    {
        public decimal Amount { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Deposit([FromBody] DepositRequestModel model)
    {
        if (model.Amount <= 0)
        {
            return Json(new { success = false, message = "Amount must be greater than zero." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _requestService.CreateRequestAsync(userId, TransactionType.Deposit, model.Amount);
            return Json(new { success = true, message = "Deposit request submitted successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }

    // Withdraw Page
    [HttpGet]
    public IActionResult Withdraw()
    {
        return View();
    }

    // Handle Withdraw Submission
    [HttpPost]
    public async Task<IActionResult> Withdraw([FromBody] DepositRequestModel model)
    {
        if (model.Amount <= 0)
        {
            return Json(new { success = false, message = "Amount must be greater than zero." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _requestService.CreateRequestAsync(userId, TransactionType.Withdraw, model.Amount);
            return Json(new { success = true, message = "Withdrawal request submitted successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
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
