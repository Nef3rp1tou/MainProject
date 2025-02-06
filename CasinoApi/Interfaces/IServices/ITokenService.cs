using CasinoApi.DTOs;

namespace CasinoApi.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<Guid> CreatePrivateTokenAsync(TokenRequestDto token);
    }
}
