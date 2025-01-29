using Dapper;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;

namespace MvcProject.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnection _dbConnection;

        public TransactionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task DepositAsync(Transactions transaction)
        {
            var sql = "[dbo].[spDeposit]";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", transaction.Id);
            parameters.Add("@UserId", transaction.UserId);
            parameters.Add("@TransactionType", transaction.TransactionType);
            parameters.Add("@Amount", transaction.Amount);
            try
            {
                await _dbConnection.ExecuteAsync(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the transaction.", ex);
            }
        }
        public async Task WithdrawAsync(Transactions transaction)
        {
            var sql = "[dbo].[spWithdraw]";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", transaction.Id);
            parameters.Add("@UserId", transaction.UserId);
            parameters.Add("@TransactionType", transaction.TransactionType);
            parameters.Add("@Amount", transaction.Amount);
            parameters.Add("@Status", transaction.Status); 

            try
            {
                await _dbConnection.ExecuteAsync(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the transaction.", ex);
            }
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM Transactions WHERE UserId = @UserId";
            return await _dbConnection.QueryAsync<Transactions>(sql, new { UserId = userId });
        }
    }
}
