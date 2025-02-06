using CasinoApi.DTOs;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Interfaces.IServices;
using CasinoApi.Utilities;

namespace CasinoApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GetBalanceResponseDto> GetBalanceAsync(GetBalanceRequestDto request)
        {

            return await _userRepository.GetBalanceAsync(request.Token, CurrencyHelper.ConvertToCurrency(request.Currency));
        }

        public async Task<UserInfoResponseDto> GetPlayerInfoAsync(Guid token)
        {
            return await _userRepository.GetPlayerInfoAsync(token);
        }
    }
}
