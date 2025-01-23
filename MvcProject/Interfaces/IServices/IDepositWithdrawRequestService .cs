using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface IDepositWithdrawRequestsService
    {
        Task CreateRequestAsync(Guid transactionId, string userId, TransactionType type, decimal amount);
        Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId);
        Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id);
        Task UpdateRequestStatusAsync(Guid id, Status status);
        Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync();
        //Task<string> SendDepositToBankingApiAsync(Guid transactionId, decimal amount);
        //Task SendWithdrawToBankingApiAsync(Guid transactionId, decimal amount, string userId, string accountNumber, string fullName);

    }
}
