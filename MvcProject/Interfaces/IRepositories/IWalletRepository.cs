using MvcProject.Models;

namespace MvcProject.Interfaces.IRepositories
{
    public interface IWalletRepository
    {
        Task<Wallet> GetWalletByUserIdAsync(string userId);
        Task CreateWalletAsync(Wallet wallet);
        Task UpdateWalletBalanceAsync(string userId, decimal newBalance);
    }
}
