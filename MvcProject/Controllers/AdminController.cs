using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Interfaces.IServices;

namespace MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var pendingRequests = await _adminService.GetPendingRequestsAsync();
            return View(pendingRequests);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest([FromBody] int requestId)
        {
            var result = await _adminService.ApproveRequestAsync(requestId);
            return Json(new { success = result.IsSuccess, message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest([FromBody] int requestId)
        {
            var result = await _adminService.RejectRequestAsync(requestId);
            return Json(new { success = result.IsSuccess, message = result.Message });
        }
    }
}
