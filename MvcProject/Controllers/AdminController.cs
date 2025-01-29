using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using NuGet.Protocol;


namespace MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDepositWithdrawRequestsService _requestService;
        private readonly IBankingApiService _bankingApiService;
        public AdminController(IDepositWithdrawRequestsService requestService, IBankingApiService bankingApiService)
        {
            _requestService = requestService;
            _bankingApiService = bankingApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var pendingRequests = await _requestService.GetPendingRequestsAsync();
            return View(pendingRequests); 
        }

        // Approve a Request
        [HttpPost]
        public async Task<IActionResult> ApproveRequest([FromBody] Guid requestId)
        {
            try
            {
                var request = await _requestService.GetRequestByIdAsync(requestId);

                if (request == null || request.TransactionType != TransactionType.Withdraw || request.Status != Status.Pending)
                {
                    return Json(new { success = false, message = "Invalid request or already processed." });
                }

                var response = await _bankingApiService.SendWithdrawRequestAsync(
                    request.Id, request.Amount
                );

                return Json(new
                {
                    success = response.Status == Status.Success,
                    message = response.Status == Status.Success
                        ? "Withdrawal request sent to Banking API. Awaiting confirmation."
                        : "Bank rejected the transaction."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> RejectRequest([FromBody] Guid requestId)
        {
            try
            {
                var request = await _requestService.GetRequestByIdAsync(requestId);

                if (request == null || request.TransactionType != TransactionType.Withdraw || request.Status != Status.Pending)
                {
                    return Json(new { success = false, message = "Invalid request or already processed." });
                }
                
                await _requestService.RejectRequestAsync(request.Id, request.UserId, request.Amount, request.TransactionType);
                return Json(new { success = true, message = "Request rejected successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
