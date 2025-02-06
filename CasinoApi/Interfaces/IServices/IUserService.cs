using CasinoApi.DTOs;

namespace CasinoApi.Interfaces.IServices
{
    public interface IUserService
    {
        Task<GetBalanceResponseDto> GetBalanceAsync(GetBalanceRequestDto request);
        Task<UserInfoResponseDto> GetPlayerInfoAsync(Guid token);
    }
}
