using Dapper;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;

namespace MvcProject.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly IDbConnection _dbConnection;

        public WalletRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM Wallet WHERE UserId = @UserId";
            return await _dbConnection.QuerySingleOrDefaultAsync<Wallet>(sql, new { UserId = userId });
        }

        public async Task CreateWalletAsync(Wallet wallet)
        {
            var sql = @"INSERT INTO Wallet (Id, UserId, CurrentBalance, Currency)
                    VALUES (@Id, @UserId, @CurrentBalance, @Currency)";
            wallet.Id = Guid.NewGuid();
            await _dbConnection.ExecuteAsync(sql, wallet);
        }

        public async Task UpdateWalletBalanceAsync(string userId, decimal newBalance, IDbTransaction transaction = null)
        {
            var sql = @"
        UPDATE Wallet 
        SET CurrentBalance = @NewBalance 
        WHERE UserId = @UserId";

            if (transaction == null)
            {
                await _dbConnection.ExecuteAsync(sql, new { NewBalance = newBalance, UserId = userId });
            }
            else
            {
                await _dbConnection.ExecuteAsync(sql, new { NewBalance = newBalance, UserId = userId }, transaction);
            }
        }

    }
}
