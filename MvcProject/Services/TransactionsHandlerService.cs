using MvcProject.Interfaces.IServices;
using log4net;
using MvcProject.Utilities;
using Microsoft.Data.SqlClient;

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

        public async Task<ServiceResult> HandleDepositAsync(string userId, decimal amount)
        {
            if (amount <= 0)
                return ServiceResult.Failure("Amount must be greater than zero.");

            var transactionId = await _requestService.RegisterDepositRequestAsync(userId, amount);
            var response = await _bankingApiService.SendDepositRequestAsync(transactionId, amount);

            return ServiceResult.Success("Deposit initiated successfully.", data: response.PaymentUrl);

        }

        public async Task<ServiceResult> HandleWithdrawAsync(string userId, decimal amount)
        {
            if (amount <= 0)
                return ServiceResult.Failure("Amount must be greater than zero.");

            var wallet = await _walletService.GetWalletByUserIdAsync(userId);
            if (wallet.CurrentBalance < amount)
                return ServiceResult.Failure("Insufficient Balance.");

            var transactionId = await _requestService.RegisterWithdrawRequestAsync(userId, amount);
            return ServiceResult.Success("Withdrawal request submitted successfully and is awaiting admin approval.");

        }
    }

}
