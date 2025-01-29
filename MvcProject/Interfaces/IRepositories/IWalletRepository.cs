using MvcProject.Models;
using System.Data;

namespace MvcProject.Interfaces.IRepositories
{
    public interface IWalletRepository
    {
        Task<Wallet> GetWalletByUserIdAsync(string userId);
        Task CreateWalletAsync(Wallet wallet);
        
    }
}
