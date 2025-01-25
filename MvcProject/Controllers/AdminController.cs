using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Services;
using System;
using System.Threading.Tasks;

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
                await _bankingApiService.SendWithdrawRequestAsync(
                    request.Id, request.Amount
                );

                return Json(new { success = true, message = "Withdrawal request sent to Banking API. Awaiting confirmation." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


        // Reject a Request
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

                await _requestService.UpdateRequestStatusAsync(requestId, Status.Rejected);
                return Json(new { success = true, message = "Request rejected successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
