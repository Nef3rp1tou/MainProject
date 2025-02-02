using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Utilities;

namespace MvcProject.Services
{
    public class AdminService : IAdminService
    {
        private readonly IDepositWithdrawRequestsRepository _requestRepository;
        private readonly IBankingApiService _bankingApiService;

        public AdminService(IDepositWithdrawRequestsRepository requestRepository, IBankingApiService bankingApiService)
        {
            _requestRepository = requestRepository;
            _bankingApiService = bankingApiService;
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            return await _requestRepository.GetPendingRequestsAsync();
        }

        public async Task<ServiceResult> ApproveRequestAsync(int requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);

            if (request == null || request.TransactionType != TransactionType.Withdraw || request.Status != Status.Pending)
            {
                return ServiceResult.Failure("Invalid request or already processed.");
            }

            var response = await _bankingApiService.SendWithdrawRequestAsync(request.Id, request.Amount);

            if (response.Status == Status.Success)
            {
                return ServiceResult.Success("Withdrawal request sent to Banking API. Awaiting confirmation.");
            }

            return ServiceResult.Failure("Bank rejected the transaction.");
        }

        public async Task<ServiceResult> RejectRequestAsync(int requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);

            if (request == null || request.TransactionType != TransactionType.Withdraw || request.Status != Status.Pending)
            {
                return ServiceResult.Failure("Invalid request or already processed.");
            }

            await _requestRepository.RejectRequestAsync(request);
            return ServiceResult.Success("Request rejected successfully!");
        }
    }
}
