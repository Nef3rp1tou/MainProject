using Dapper;
using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;

namespace MvcProject.Repositories
{
    public class DepositWithdrawRequestsRepository(IDbConnection dbConnection) : IDepositWithdrawRequestsRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task CreateRequestAsync(DepositWithdrawRequests request)
        {
            var sql = @"
            INSERT INTO DepositWithdrawRequests (Id, UserId, TransactionType, Amount, Status, CreatedAt)
            VALUES (@Id, @UserId, @TransactionType, @Amount, @Status, @CreatedAt)";

            await _dbConnection.ExecuteAsync(sql, request);
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE UserId = @UserId";
            return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { UserId = userId });
        }

        public async Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<DepositWithdrawRequests>(sql, new { Id = id });
        }

        public async Task UpdateRequestStatusAsync(Guid id, Status status)
        {
            var sql = @"
                UPDATE DepositWithdrawRequests
                SET Status = @Status
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Status = (byte)status, Id = id });

        }
        
        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
           var sql = "SELECT * FROM DepositWithdrawRequests WHERE Status = @PendingStatus";
            return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { PendingStatus = (byte)Status.Pending });
        }
    }
}
