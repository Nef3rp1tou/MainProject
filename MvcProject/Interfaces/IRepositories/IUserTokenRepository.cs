using MvcProject.Models;

namespace MvcProject.Interfaces.IRepositories
{
    public interface IUserTokenRepository
    {
        Task<Guid> GeneratePublicToken(string userId);
    }

}
