using Microsoft.AspNetCore.Mvc;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Enums;
namespace MvcProject.Controllers
{



    public class CallbackController : ControllerBase
    {
        private readonly IDepositWithdrawRequestsService _requestService;
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        public CallbackController(
            IDepositWithdrawRequestsService requestService,
            IWalletService walletService,
            ITransactionService transactionService)
        {
            _requestService = requestService;
            _walletService = walletService;
            _transactionService = transactionService;
        }


        [HttpPost("callback/handle")]
        public async Task<IActionResult> HandleCallback([FromBody] CallbackRequestModel callbackRequest)
        {
            if (callbackRequest == null || callbackRequest.TransactionId == Guid.Empty)
            {
                return BadRequest("Invalid callback request.");
            }

            try
            {
                // Fetch the transaction request
                var request = await _requestService.GetRequestByIdAsync(callbackRequest.TransactionId);
                if (request == null)
                {
                    return NotFound("Transaction not found.");
                }

                // Update the transaction status
                await _requestService.UpdateRequestStatusAsync(callbackRequest.TransactionId, callbackRequest.Status);

                if (callbackRequest.Status == Status.Success)
                {
                    // Register the transaction in the Transactions table
                    var transaction = new Transactions
                    {
                        Id = Guid.NewGuid(),
                        UserId = request.UserId,
                        Amount = request.Amount,
                        Status = Status.Success,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _transactionService.CreateTransactionAsync(transaction);

                    // Update wallet balance for deposits
                    if (request.TransactionType == TransactionType.Deposit)
                    {
                        var wallet = await _walletService.GetWalletByUserIdAsync(request.UserId);
                        var updatedBalance = wallet.CurrentBalance + request.Amount;
                        await _walletService.UpdateWalletBalanceAsync(request.UserId, updatedBalance);
                    }
                }

                return Ok("Callback processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the callback: {ex.Message}");
            }
        }

    }

}
