using Dapper;
using Microsoft.Data.SqlClient;
using MvcProject.DTOs;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;
using log4net;
using MvcProject.Enums;
using MvcProject.Utilities;

namespace MvcProject.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnection _dbConnection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TransactionRepository));

        public TransactionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task DepositAsync(TransactionDto transaction)
        {
            var sql = "[dbo].[Deposit]";

            var parameters = new DynamicParameters();
            parameters.Add("@TransactionRequestId", transaction.TransactionRequestId);
            parameters.Add("@UserId", transaction.UserId);
            parameters.Add("@TransactionType", transaction.TransactionType);
            parameters.Add("@Amount", transaction.Amount);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

        }

        public async Task WithdrawAsync(TransactionDto transaction)
        {
            var sql = "[dbo].[Withdraw]";

            var parameters = new DynamicParameters();
            parameters.Add("@TransactionRequestId", transaction.TransactionRequestId);
            parameters.Add("@UserId", transaction.UserId);
            parameters.Add("@TransactionType", transaction.TransactionType);
            parameters.Add("@Amount", transaction.Amount);
            parameters.Add("@Status", transaction.Status);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM Transactions WHERE UserId = @UserId";

            return await _dbConnection.QueryAsync<Transactions>(sql, new { UserId = userId });
           
        }
    }
}
