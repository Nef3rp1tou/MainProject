using MvcProject.Enums;
using MvcProject.Models;
using System.Threading.Tasks;

namespace MvcProject.Interfaces.IServices
{
    public interface IDepositWithdrawRequestsService
    {
        Task<int> RegisterDepositRequestAsync(string userId, decimal amount);
        Task<int> RegisterWithdrawRequestAsync(string userId, decimal amount);
        Task RejectRequestAsync(int transactionId, string userId, decimal amount, TransactionType transactionType);
        Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId);
        Task<DepositWithdrawRequests> GetRequestByIdAsync(int id);
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
       
    }
}
