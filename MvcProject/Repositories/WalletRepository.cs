using Dapper;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;
using log4net;
using Microsoft.Data.SqlClient;
using MvcProject.Enums;
using MvcProject.Utilities;

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
            try
            {
                var sql = "SELECT * FROM Wallet WHERE UserId = @UserId";

                var wallet = await _dbConnection.QuerySingleOrDefaultAsync<Wallet>(sql, new { UserId = userId });

                if (wallet == null) {
                    throw new CustomException(CustomStatusCode.UserNotFound);
                }

                return wallet;

            }
            catch (SqlException)
            {
                throw new CustomException(CustomStatusCode.GeneralError);
            }
        }

        public async Task CreateWalletAsync(Wallet wallet)
        {
            var sql = "[dbo].[CreateWallet]";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", wallet.UserId);
            parameters.Add("@Currency", wallet.Currency);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }
        }
    }
}
