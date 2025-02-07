using MvcProject.Utilities;

namespace MvcProject.Interfaces.IServices
{
    public interface ITransactionsHandlerService
    {
        Task<CustomResponse> HandleDepositAsync(string userId, decimal amount);
        Task<CustomResponse> HandleWithdrawAsync(string userId, decimal amount);
    }
}
