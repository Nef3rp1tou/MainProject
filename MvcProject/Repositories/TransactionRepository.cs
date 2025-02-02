using Dapper;
using Microsoft.Data.SqlClient;
using MvcProject.DTOs;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;
using log4net;

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

            try
            {
                await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.Error($"SQL Error {ex.Number}: {ex.Message}", ex);

                if (ex.Number == 50009)
                    throw new InvalidOperationException("User has a pending transaction. Deposit not allowed.");

                throw new Exception("An error occurred while processing the deposit.", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected Error: {ex.Message}", ex);
                throw new Exception("An unexpected error occurred while processing the deposit.", ex);
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

            try
            {
                await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.Error($"SQL Error {ex.Number}: {ex.Message}", ex);

                if (ex.Number == 50009)
                    throw new InvalidOperationException("User has a pending transaction. Withdrawal not allowed.");
                if (ex.Number == 50008)
                    throw new InvalidOperationException("Blocked amount is insufficient to complete withdrawal.");

                throw new Exception("An error occurred while processing the withdrawal.", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected Error: {ex.Message}", ex);
                throw new Exception("An unexpected error occurred while processing the withdrawal.", ex);
            }
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM Transactions WHERE UserId = @UserId";

            try
            {
                return await _dbConnection.QueryAsync<Transactions>(sql, new { UserId = userId });
            }
            catch (SqlException ex)
            {
                _logger.Error($"SQL Error {ex.Number}: {ex.Message}", ex);
                throw;
            }
        }
    }
}
