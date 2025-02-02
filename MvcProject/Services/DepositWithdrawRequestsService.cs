using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;

namespace MvcProject.Services
{
    public class DepositWithdrawRequestsService : IDepositWithdrawRequestsService
    {
        private readonly IDepositWithdrawRequestsRepository _requestRepository;

        public DepositWithdrawRequestsService(IDepositWithdrawRequestsRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<int> RegisterDepositRequestAsync(string userId, decimal amount)
        {
            var request = new DepositWithdrawRequests
            {
                UserId = userId,
                Amount = amount,
                TransactionType = TransactionType.Deposit
            };

            var transactionId = await _requestRepository.RegisterTransactionAsync(request);
            return transactionId;
        }

        public async Task<int> RegisterWithdrawRequestAsync(string userId, decimal amount)
        {
            var request = new DepositWithdrawRequests
            {
                UserId = userId,
                Amount = amount,
                TransactionType = TransactionType.Withdraw
            };
            var transactionId = await _requestRepository.RegisterTransactionAsync(request);
            return transactionId;
        }

        public async Task RejectRequestAsync(int transactionId, string userId, decimal amount, TransactionType transactionType)
        {
            var request = new DepositWithdrawRequests
            {
                Id = transactionId,
                UserId = userId,
                Amount = amount,
                TransactionType = transactionType
            };
            await _requestRepository.RejectRequestAsync(request);
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId)
        {
            return await _requestRepository.GetRequestsByUserIdAsync(userId);
        }

        public async Task<DepositWithdrawRequests> GetRequestByIdAsync(int id)
        {
            return await _requestRepository.GetRequestByIdAsync(id);
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            return await _requestRepository.GetPendingRequestsAsync();
        }
    }
}
