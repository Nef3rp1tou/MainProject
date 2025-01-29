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
      

        public DepositWithdrawRequestsService(
          IDepositWithdrawRequestsRepository requestRepository)
        {
            _requestRepository = requestRepository;
      
        }
  
        public async Task RegisterDepositRequestAsync(Guid transactionId, string userId, decimal amount)
        {
            var request = new DepositWithdrawRequests
            {
                Id = transactionId,
                UserId = userId,
                Amount = amount,
            };

            await _requestRepository.RegisterDepositRequestAsync(request);
        }

        public async Task RegisterWithdrawRequestAsync(Guid transactionId, string userId, decimal amount)
        {
            var request = new DepositWithdrawRequests
            {
                Id = transactionId,
                UserId = userId,
                Amount = amount,
            };
            await _requestRepository.RegisterWithdrawRequestAsync(request);
        }
        public async Task RejectRequestAsync(Guid transactionId, string userId, decimal amount, TransactionType transactionType)
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

        public async Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id)
        {
            return await _requestRepository.GetRequestByIdAsync(id);
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            return await _requestRepository.GetPendingRequestsAsync();
        }
  
    }
}
