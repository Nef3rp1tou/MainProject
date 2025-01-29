using MvcProject.Models;
using System.Threading.Tasks;

namespace MvcProject.Interfaces.IRepositories
{
    public interface ITransactionRepository
    {
        Task DepositAsync(Transactions transaction);
        Task WithdrawAsync(Transactions transaction);
        Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId);
    }
}
