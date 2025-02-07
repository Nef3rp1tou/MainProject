using MvcProject.DTOs;
using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{

    public interface IWalletService
    {
        Task<WalletResponseDto> GetWalletByUserIdAsync(string userId);
        Task CreateWalletForUserAsync(string userId, int currency);
   
    }
}
