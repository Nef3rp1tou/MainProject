using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AdminController(IDepositWithdrawRequestsService requestService)
        {
            _requestService = requestService;
        }

        // Admin Dashboard - Display Pending Requests
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var pendingRequests = await _requestService.GetPendingRequestsAsync();
            return View(pendingRequests); // Pass the pending requests to the view
        }

        // Approve a Request
        [HttpPost]
        public async Task<IActionResult> ApproveRequest([FromBody] Guid requestId)
        {
            try
            {
                await _requestService.ApproveRequestAsync(requestId);
                return Json(new { success = true, message = "Request approved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // Reject a Request
        [HttpPost]
        public async Task<IActionResult> RejectRequest([FromBody] Guid requestId)
        {
            try
            {
                await _requestService.RejectRequestAsync(requestId);
                return Json(new { success = true, message = "Request rejected successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
