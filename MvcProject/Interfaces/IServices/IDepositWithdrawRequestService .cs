using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface IDepositWithdrawRequestsService
    {
        Task CreateRequestAsync(string userId, TransactionType type, decimal amount);
        Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId);
        Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id);
        Task UpdateRequestStatusAsync(Guid id, Status status);
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
        Task ApproveRequestAsync(Guid requestId);

        Task RejectRequestAsync(Guid requestId);

        Task HandleWithdrawRequestAsync(string userId, decimal amount);
    }
}
