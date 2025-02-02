using MvcProject.DTOs;
using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface ITransactionService
    {
        Task DepositAsync(TransactionDto transaction);
        Task WithdrawAsync(TransactionDto transaction);

        Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId);
    }
}
