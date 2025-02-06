using CasinoApi.DTOs;
using CasinoApi.Enums;

namespace CasinoApi.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<UserInfoResponseDto> GetPlayerInfoAsync(Guid token);
        Task<GetBalanceResponseDto> GetBalanceAsync(Guid token, Currency currency);
    }
}
