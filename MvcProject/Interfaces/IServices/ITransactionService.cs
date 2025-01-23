using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface ITransactionService
    {
        Task CreateTransactionAsync(Transactions transaction);
        Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId);
    }
}
