using MvcProject.Utilities;

namespace MvcProject.Interfaces.IServices
{
    public interface ITransactionsHandlerService
    {
        Task<ServiceResult> HandleDepositAsync(string userId, decimal amount);
        Task<ServiceResult> HandleWithdrawAsync(string userId, decimal amount);
    }
}
