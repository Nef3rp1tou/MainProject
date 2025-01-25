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

        public async Task CreateTransactionAsync(Transactions transaction)
        {
            var sql = "[dbo].[CreateTransaction]";
            
            var parameters = new
            {
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                Status = (byte)transaction.Status
            };

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
            var sql = "[dbo].[GetTransactionsByUserId]";

            var parameters = new { UserId = userId };

            try
            {
                return await _dbConnection.QueryAsync<Transactions>(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving transactions for the user.", ex);
            }
        }
    }
}
