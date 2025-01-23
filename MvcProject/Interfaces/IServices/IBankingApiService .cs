using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface IBankingApiService
    {
        Task<ApiResponse> SendDepositRequestAsync(Guid transactionId, decimal amount);
        Task<ApiResponse> SendDepositFinishRequestAsync(Guid transactionId, decimal amount);

        Task<ApiResponse> SendWithdrawRequestAsync(Guid transactionId, decimal amount, string userId, string accountNumber, string fullName);
        bool ValidateHash(string receivedHash, string expectedData);
    }
}
