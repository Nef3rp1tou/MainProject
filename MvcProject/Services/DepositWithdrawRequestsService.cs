using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Repositories;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MvcProject.Services
{
    public class DepositWithdrawRequestsService : IDepositWithdrawRequestsService
    {
        private readonly IDepositWithdrawRequestsRepository _requestRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IDbConnection _dbConnection;
        public DepositWithdrawRequestsService(
          IDepositWithdrawRequestsRepository requestRepository,
          IWalletRepository walletRepository,
          IDbConnection dbConnection)
        {
            _requestRepository = requestRepository;
            _walletRepository = walletRepository;
            _dbConnection = dbConnection;
        }
        public async Task CreateRequestAsync(string userId, TransactionType type, decimal amount)
        {
            var request = new DepositWithdrawRequests
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TransactionType = type,
                Amount = amount,
                Status = Status.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _requestRepository.CreateRequestAsync(request);
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId)
        {
            return await _requestRepository.GetRequestsByUserIdAsync(userId);
        }

        public async Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id)
        {
            return await _requestRepository.GetRequestByIdAsync(id);
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            return await _requestRepository.GetPendingRequestsAsync();
        }

        public async Task UpdateRequestStatusAsync(Guid id, Status status)
        {
            var request = await _requestRepository.GetRequestByIdAsync(id);

            if (request == null)
                throw new InvalidOperationException("Request not found.");

            // Optional: Add validation logic
            if (request.Status == Status.Success || request.Status == Status.Rejected)
                throw new InvalidOperationException("Cannot update status of a processed request.");

            // Update the status in the repository
            await _requestRepository.UpdateRequestStatusAsync(id, status);
        }

        public async Task ApproveRequestAsync(Guid requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
            if (request == null)
            {
                Console.WriteLine($"Request with ID {requestId} not found.");
                throw new InvalidOperationException("Invalid or already processed request.");
            }

            if (request.Status != Status.Pending)
            {
                Console.WriteLine($"Request with ID {requestId} has already been processed with status {request.Status}.");
                throw new InvalidOperationException("Invalid or already processed request.");
            }

            await _requestRepository.UpdateRequestStatusAsync(requestId, Status.Success);
        }

        public async Task RejectRequestAsync(Guid requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
            if (request == null || request.Status != Status.Pending)
                throw new InvalidOperationException("Invalid or already processed request.");

            await _requestRepository.UpdateRequestStatusAsync(requestId, Status.Rejected);
        }

        public async Task HandleWithdrawRequestAsync(string userId, decimal amount)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
                    if (wallet.CurrentBalance < amount)
                        throw new InvalidOperationException("Insufficient balance.");

                    var newBalance = wallet.CurrentBalance - amount;
                    await _walletRepository.UpdateWalletBalanceAsync(userId, newBalance, transaction);

                    var request = new DepositWithdrawRequests
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        TransactionType = TransactionType.Withdraw,
                        Amount = amount,
                        Status = Status.Success,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _requestRepository.CreateRequestAsync(request);
                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

    }
}
