using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IRepositories
{
    public interface IDepositWithdrawRequestsRepository
    {
        Task RegisterDepositRequestAsync(DepositWithdrawRequests request);
        Task RegisterWithdrawRequestAsync(DepositWithdrawRequests request);
        Task RejectRequestAsync(DepositWithdrawRequests requests);
        Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId);
        Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id);
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
    }
}
