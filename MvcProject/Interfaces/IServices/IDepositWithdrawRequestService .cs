using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface IDepositWithdrawRequestsService
    {
        Task RegisterDepositRequestAsync(Guid transactionId, string userId, decimal amount);
        Task RegisterWithdrawRequestAsync(Guid transactionId, string userId, decimal amount);
        Task RejectRequestAsync(Guid transactionId, string userId, decimal amount, TransactionType transactionType);
        Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId);
        Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id);
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
       
    }
}
