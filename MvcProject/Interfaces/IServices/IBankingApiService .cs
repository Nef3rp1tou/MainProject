using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{
    public interface IBankingApiService
    {
        Task<ApiResponse> SendDepositRequestAsync(Guid transactionId, decimal amount);
        Task<DepositFinishRequestDto> SendDepositFinishRequestAsync(Guid transactionId, decimal amount);

        Task<WithdrawRequestDto> SendWithdrawRequestAsync(Guid transactionId, decimal amount);
        bool ValidateHash(string receivedHash, string expectedData);
    }
}
