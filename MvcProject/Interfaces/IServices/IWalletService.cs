using MvcProject.Models;

namespace MvcProject.Interfaces.IServices
{

    public interface IWalletService
    {
        Task<Wallet> GetWalletByUserIdAsync(string userId);
        Task CreateWalletForUserAsync(string userId, int currency);
        Task UpdateWalletBalanceAsync(string userId, decimal newBalance);
        Task BlockBalanceAsync(string userId, decimal amount);
        Task UnblockBalanceAsync(string userId, decimal amount);
    }
}
