using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IRepositories
{
    public interface IDepositWithdrawRequestsRepository
    {
        Task CreateRequestAsync(DepositWithdrawRequests request);
        Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId);
        Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id);
        Task UpdateRequestStatusAsync(Guid id, Status status);
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
    }
}
