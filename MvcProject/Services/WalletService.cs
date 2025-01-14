using Dapper;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using System.Data;

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

        public async Task CreateWalletForUserAsync(string userId)
        {
            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CurrentBalance = 0, // Initial balance
                Currency = Enums.Currency.EUR // Initial currency
            };

            await _walletRepository.CreateWalletAsync(wallet);
        }

        public async Task UpdateWalletBalanceAsync(string userId, decimal newBalance)
        {
            await _walletRepository.UpdateWalletBalanceAsync(userId, newBalance);
        }


       
    }
}
