using Dapper;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;
using log4net;
using Microsoft.Data.SqlClient;

namespace MvcProject.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly IDbConnection _dbConnection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(WalletRepository));

        public WalletRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM Wallet WHERE UserId = @UserId";

            try
            {
                return await _dbConnection.QuerySingleOrDefaultAsync<Wallet>(sql, new { UserId = userId });
            }
            catch (SqlException ex)
            {
                _logger.Error($"SQL Error {ex.Number}: {ex.Message}", ex);
                throw new Exception("An error occurred while fetching the wallet.", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected Error: {ex.Message}", ex);
                throw new Exception("An unexpected error occurred while fetching the wallet.", ex);
            }
        }

        public async Task CreateWalletAsync(Wallet wallet)
        {
            var sql = "[dbo].[CreateWallet]";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", wallet.UserId);
            parameters.Add("@Currency", wallet.Currency);

            try
            {
                await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.Error($"SQL Error {ex.Number}: {ex.Message}", ex);
                throw new Exception("An error occurred while creating the wallet.", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected Error: {ex.Message}", ex);
                throw new Exception("An unexpected error occurred while creating the wallet.", ex);
            }
        }
    }
}
