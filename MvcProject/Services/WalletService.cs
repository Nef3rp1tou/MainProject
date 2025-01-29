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

    }
}
