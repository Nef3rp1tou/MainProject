using MvcProject.Models;

namespace MvcProject.Interfaces.IRepositories
{
    public interface ITransactionRepository
    {
        Task CreateTransactionAsync(Transactions transaction);
        Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId);
    }
}
