using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IRepositories
{
    public interface IDepositWithdrawRequestsRepository
    {
        Task<int> RegisterTransactionAsync(DepositWithdrawRequests request);
        Task RejectRequestAsync(DepositWithdrawRequests requests);
        Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId);
        Task<DepositWithdrawRequests> GetRequestByIdAsync(int id);
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
    }
}
