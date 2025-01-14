using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{

    public interface IWalletService
    {
        Task<Wallet> GetWalletByUserIdAsync(string userId);
        Task CreateWalletForUserAsync(string userId);
        Task UpdateWalletBalanceAsync(string userId, decimal newBalance);
    }
}
