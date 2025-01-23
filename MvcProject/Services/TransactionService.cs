using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;

namespace MvcProject.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task CreateTransactionAsync(Transactions transaction)
        {
            await _transactionRepository.CreateTransactionAsync(transaction);
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId)
        {
            return await _transactionRepository.GetTransactionsByUserIdAsync(userId);
        }
    }
}
