using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Models;
using MvcProject.Utilities;

namespace MvcProject.Interfaces.IServices
{
    public interface IAdminService
    {
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
        Task<CustomResponse> ApproveRequestAsync(int requestId);
        Task<CustomResponse> RejectRequestAsync(int requestId);
    }
}
