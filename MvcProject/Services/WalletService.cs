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
                UserId = userId,
                Currency = (Enums.Currency)currency
            };

            await _walletRepository.CreateWalletAsync(wallet);
        }

        public async Task UpdateWalletBalanceAsync(string userId, decimal newBalance)
        {
            await _walletRepository.UpdateWalletBalanceAsync(userId, newBalance);
        }

        public async Task BlockBalanceAsync(string userId, decimal amount)
        {
            // Get the current wallet state
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found.");
            }

            // Calculate the new blocked balance
            var newBlockedBalance = wallet.BlockedAmount + amount;

            // Update the blocked balance
            await _walletRepository.UpdateBlockedBalanceAsync(userId, newBlockedBalance);
        }

        public async Task UnblockBalanceAsync(string userId, decimal amount)
        {
            // Get the current wallet state
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found.");
            }

            // Ensure there are enough funds in the blocked balance to unblock
            if (wallet.BlockedAmount < amount)
            {
                throw new Exception("Insufficient blocked balance.");
            }

            // Calculate the new blocked balance
            var newBlockedBalance = wallet.BlockedAmount - amount;

            // Update the blocked balance
            await _walletRepository.UpdateBlockedBalanceAsync(userId, newBlockedBalance);
        }

    }
}
