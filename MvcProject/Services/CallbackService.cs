using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Utilities;

namespace MvcProject.Services
{
    public class CallbackService : ICallbackService
    {
        private readonly IDepositWithdrawRequestsRepository _requestRepository;
        private readonly ITransactionService _transactionService;

        public CallbackService(
            IDepositWithdrawRequestsRepository requestRepository,
            ITransactionService transactionService)
        {
            _requestRepository = requestRepository;
            _transactionService = transactionService;
        }

        public async Task<ServiceResult> ProcessCallbackAsync(CallbackRequestModel callbackRequest, bool isWithdraw)
        {
            if (callbackRequest == null || callbackRequest.TransactionId <= 0)
                return ServiceResult.Failure("Invalid callback request.");

            var request = await _requestRepository.GetRequestByIdAsync(callbackRequest.TransactionId);
            if (request == null)
                return ServiceResult.Failure("Transaction not found.");

            if (callbackRequest.Status == Status.Success)
            {
                var transaction = new TransactionDto
                {
                    TransactionRequestId = request.Id,
                    UserId = request.UserId,
                    TransactionType = request.TransactionType,
                    Amount = request.Amount,
                    Status = Status.Success
                };

                if (isWithdraw)
                    await _transactionService.WithdrawAsync(transaction);
                else
                    await _transactionService.DepositAsync(transaction);

                return ServiceResult.Success("Callback processed successfully.");
            }

            return ServiceResult.Failure("Callback status is not successful.");
        }
    }
}
