using MvcProject.DTOs;

namespace MvcProject.Interfaces.IServices
{
    public interface IUserTokenService
    {
        Task<Guid> GeneratePublicToken(string userId);
    }


}
