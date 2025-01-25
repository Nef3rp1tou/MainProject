using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using System.Threading.Tasks;

namespace MvcProject.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(string userId)
        {
            return await _walletRepository.GetWalletByUserIdAsync(userId);
        }

        public async Task CreateWalletForUserAsync(string userId, int currency)
        {
            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CurrentBalance = 0, // Initial balance
                Currency = (Enums.Currency)currency
            };

            await _walletRepository.CreateWalletAsync(wallet);
        }

        public async Task UpdateWalletBalanceAsync(string userId, decimal newBalance)
        {
            await _walletRepository.UpdateWalletBalanceAsync(userId, newBalance);
        }

  

        public async Task UnlockBlockedAmountAsync(string userId, decimal amount)
        {
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            var updatedBalance = wallet.CurrentBalance + amount; 
            await UpdateWalletBalanceAsync(userId, updatedBalance);
        }
    }
}
