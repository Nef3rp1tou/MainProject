using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface ITransactionService
    {
        Task DepositAsync(Transactions transaction);
        Task WithdrawAsync(Transactions transaction);

        Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId);
    }
}
