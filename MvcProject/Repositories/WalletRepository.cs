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
            var sql = "[dbo].[CreateWallet]";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", wallet.UserId);
            parameters.Add("@Currency", (byte)wallet.Currency);

            try
            {
                await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the wallet.", ex);
            }
        }

    }
}
