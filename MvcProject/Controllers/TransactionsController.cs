using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using System.Security.Claims;


[Authorize]
public class TransactionsController : Controller
{
    private readonly IDepositWithdrawRequestsService _requestService;
    private readonly IBankingApiService _bankingApiService;
    private readonly ITransactionService _transactionService;
    private readonly IWalletService _walletService;


    public TransactionsController(IDepositWithdrawRequestsService requestService, IBankingApiService bankingApiService, ITransactionService transactionService, IWalletService walletService)
    {
        _requestService = requestService;
        _bankingApiService = bankingApiService;
        _transactionService = transactionService;
        _walletService = walletService;
    }


    [HttpGet]
    public IActionResult Deposit()
    {
        return View();
    }




    [HttpPost]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequestDto model)
    {
        decimal amount = model.Amount;
        if (amount <= 0)
        {
            return Json(new { success = false, message = "Amount must be greater than zero." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "User is not authenticated." });
        }

        try
        {
            var transactionId = Guid.NewGuid();

            await _requestService.RegisterDepositRequestAsync(transactionId, userId, amount);

            var response = _bankingApiService.SendDepositRequestAsync(transactionId, amount);

       
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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wallet = await _walletService.GetWalletByUserIdAsync(userId);
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "User is not authenticated." });
        }

        try
        {
            if (model.Amount <= wallet.CurrentBalance)
            {
                var transactionId = Guid.NewGuid();

               await _requestService.RegisterWithdrawRequestAsync(transactionId, userId, model.Amount);

                return Json(new { success = true, message = "Withdrawal request submitted successfully and is awaiting admin approval." });
            }
            
            return Json(new { success = false, message = "Insuficient Balance." });
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
