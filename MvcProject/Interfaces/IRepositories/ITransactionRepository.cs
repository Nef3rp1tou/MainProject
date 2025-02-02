using MvcProject.DTOs;
using MvcProject.Models;
using System.Threading.Tasks;

namespace MvcProject.Interfaces.IRepositories
{
    public interface ITransactionRepository
    {
        Task DepositAsync(TransactionDto transaction);
        Task WithdrawAsync(TransactionDto transaction);
        Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId);
    }
}
