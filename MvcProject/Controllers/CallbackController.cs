using Microsoft.AspNetCore.Mvc;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Enums;

namespace MvcProject.Controllers
{
    [ApiController]
    [Route("callback")]
    public class CallbackController : ControllerBase
    {
        private readonly IDepositWithdrawRequestsService _requestService;
        private readonly ITransactionService _transactionService;

        public CallbackController(
            IDepositWithdrawRequestsService requestService,
            ITransactionService transactionService)
        {
            _requestService = requestService;
            _transactionService = transactionService;
        }

        [HttpPost("handle")]
        public async Task<IActionResult> HandleCallback([FromBody] CallbackRequestModel callbackRequest)
        {
            return await ProcessCallbackAsync(callbackRequest, isWithdraw: false);
        }

        [HttpPost("handlewithdraw")]
        public async Task<IActionResult> HandleWithdraw([FromBody] CallbackRequestModel callbackRequest)
        {
            return await ProcessCallbackAsync(callbackRequest, isWithdraw: true);
        }

        private async Task<IActionResult> ProcessCallbackAsync(CallbackRequestModel callbackRequest, bool isWithdraw)
        {
            if (callbackRequest == null || callbackRequest.TransactionId == Guid.Empty)
                return BadRequest("Invalid callback request.");
           

            try
            {
                var request = await _requestService.GetRequestByIdAsync(callbackRequest.TransactionId);
                if (request == null)
                    return NotFound("Transaction not found.");

                if (callbackRequest.Status == Status.Success)
                {
                    var transaction = new Transactions
                    {
                        Id = Guid.NewGuid(),
                        UserId = request.UserId,
                        TransactionType = request.TransactionType,
                        Amount = request.Amount,
                        Status = Status.Success
                    };

                    if (isWithdraw)
                        await _transactionService.WithdrawAsync(transaction);
                    else
                        await _transactionService.DepositAsync(transaction);
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
