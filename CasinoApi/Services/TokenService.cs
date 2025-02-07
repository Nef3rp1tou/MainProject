using CasinoApi.DTOs;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Interfaces.IServices;
using CasinoApi.Utilities;

namespace CasinoApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<Guid> CreatePrivateTokenAsync(TokenRequestDto token)
        {
            if (token == null || token.PublicToken == Guid.Empty)
            {
                throw new CustomException(CustomStatusCode.InvalidRequest);
            }
            var result = await _tokenRepository.GetPrivateTokenAsync(token.PublicToken);
            return result;
        }
    }
}
