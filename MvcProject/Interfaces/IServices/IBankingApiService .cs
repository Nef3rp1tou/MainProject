using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface IBankingApiService
    {
        Task<ApiResponse> SendDepositRequestAsync(int transactionId, decimal amount);
        Task<DepositFinishRequestDto> SendDepositFinishRequestAsync(int transactionId, decimal amount);

        Task<WithdrawRequestDto> SendWithdrawRequestAsync(int transactionId, decimal amount);
        bool ValidateHash(string receivedHash, string expectedData);
    }
}
