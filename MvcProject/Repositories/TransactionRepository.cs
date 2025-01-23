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
            var sql = @"
                INSERT INTO Transactions (Id, UserId, Amount, Status, CreatedAt)
                VALUES (@Id, @UserId, @Amount, @Status, @CreatedAt)";

            transaction.Id = Guid.NewGuid();
            transaction.CreatedAt = DateTime.UtcNow;

            await _dbConnection.ExecuteAsync(sql, transaction);
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM Transactions WHERE UserId = @UserId ORDER BY CreatedAt DESC";
            return await _dbConnection.QueryAsync<Transactions>(sql, new { UserId = userId });
        }
    }
}
