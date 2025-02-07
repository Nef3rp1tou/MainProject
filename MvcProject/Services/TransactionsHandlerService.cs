using MvcProject.Interfaces.IServices;
using log4net;
using MvcProject.Utilities;
using MvcProject.Enums;

namespace MvcProject.Services
{
    public class TransactionsHandlerService : ITransactionsHandlerService
    {
        private readonly IDepositWithdrawRequestsService _requestService;
        private readonly IBankingApiService _bankingApiService;
        private readonly IWalletService _walletService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TransactionsHandlerService));

        public TransactionsHandlerService(
            IDepositWithdrawRequestsService requestService,
            IBankingApiService bankingApiService,
            IWalletService walletService)
        {
            _requestService = requestService;
            _bankingApiService = bankingApiService;
            _walletService = walletService;
        }

        public async Task<CustomResponse> HandleDepositAsync(string userId, decimal amount)
        {
            if (amount <= 0)
                throw new CustomException(CustomStatusCode.InvalidAmount);

            var transactionId = await _requestService.RegisterDepositRequestAsync(userId, amount);
            var response = await _bankingApiService.SendDepositRequestAsync(transactionId, amount);

            return new CustomResponse(CustomStatusCode.Success, response.PaymentUrl, "Deposit initiated successfully.");
        }

        public async Task<CustomResponse> HandleWithdrawAsync(string userId, decimal amount)
        {
            if (amount <= 0)
                throw new CustomException(CustomStatusCode.InvalidAmount);

            var wallet = await _walletService.GetWalletByUserIdAsync(userId);
            if (wallet.Balance < amount)
                throw new CustomException(CustomStatusCode.InsufficientBalance);

            var transactionId = await _requestService.RegisterWithdrawRequestAsync(userId, amount);
            return new CustomResponse(CustomStatusCode.Success,  message: "Withdrawal request submitted successfully and is awaiting admin approval.");

        }
    }

}
