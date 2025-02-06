namespace CasinoApi.Interfaces.IRepositories
{
    public interface ITokenRepository
    {
        Task<Guid> CreatePrivateTokenAsync(Guid publicToken);
    }
}
