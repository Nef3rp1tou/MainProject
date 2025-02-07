namespace CasinoApi.Interfaces.IRepositories
{
    public interface ITokenRepository
    {
        Task<Guid> GetPrivateTokenAsync(Guid publicToken);
    }
}
