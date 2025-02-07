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

        public async Task<CustomResponse> ApproveRequestAsync(int requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);

            var response = await _bankingApiService.SendWithdrawRequestAsync(request.Id, request.Amount);

            if (response.Status == Status.Success)
            {
                return new CustomResponse(CustomStatusCode.Success, message:"Withdrawal request sent to Banking API. Awaiting confirmation.");
            }

            throw new CustomException(CustomStatusCode.GeneralError);
        }

        public async Task<CustomResponse> RejectRequestAsync(int requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);

            await _requestRepository.RejectRequestAsync(request);
            return new CustomResponse(CustomStatusCode.Success, message:"Request rejected successfully!");
        }
    }
}
