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
using NuGet.Protocol.Core.Types;


namespace MvcProject.Services
{
    public class DepositWithdrawRequestsService : IDepositWithdrawRequestsService
    {
        private readonly IDepositWithdrawRequestsRepository _requestRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IDbConnection _dbConnection;
        private readonly IBankingApiService _bankingApiService;

        public DepositWithdrawRequestsService(
          IDepositWithdrawRequestsRepository requestRepository,
          IWalletRepository walletRepository,
          IDbConnection dbConnection, IBankingApiService bankingApiService)
        {
            _requestRepository = requestRepository;
            _walletRepository = walletRepository;
            _dbConnection = dbConnection;
            _bankingApiService = bankingApiService;

        }
        public async Task CreateRequestAsync(Guid transactionId, string userId, TransactionType type, decimal amount)
        {
            var request = new DepositWithdrawRequests
            {
                Id = transactionId,
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

            if (request.Status == Status.Success || request.Status == Status.Rejected)
                throw new InvalidOperationException("Cannot update status of a processed request.");

            await _requestRepository.UpdateRequestStatusAsync(id, status);
        }

        //public async Task<string> SendDepositToBankingApiAsync(Guid transactionId, decimal amount)
        //{
        //    var response = await _bankingApiService.SendDepositRequestAsync(transactionId, amount);

        //    if (response.Status == Status.Success && !string.IsNullOrEmpty(response.PaymentUrl))
        //    {
        //        return response.PaymentUrl;
        //    }

        //    throw new InvalidOperationException("Banking API rejected the deposit request or returned an invalid URL.");
        //}


        //public async Task SendWithdrawToBankingApiAsync(Guid transactionId, decimal amount, string userId, string accountNumber, string fullName)
        //{
        //    await _bankingApiService.SendWithdrawRequestAsync(transactionId, amount, userId, accountNumber, fullName);
        //}


     

    }
}
