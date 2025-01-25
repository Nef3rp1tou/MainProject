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
            var sql = "[dbo].[CreateDepositWithdrawRequest]";

            var parameters = new
            {
                Id = request.Id,
                UserId = request.UserId,
                TransactionType = (byte)request.TransactionType,
                Amount = request.Amount,
                Status = request.Status
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
                throw new Exception("An error occurred while creating the deposit/withdraw request.", ex);
            }
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE UserId = @UserId";
            return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { UserId = userId });
        }

        public async Task<DepositWithdrawRequests> GetRequestByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE Id = @Id";
            var result = await _dbConnection.QuerySingleOrDefaultAsync<DepositWithdrawRequests>(sql, new { Id = id });
            return result;
        }

        public async Task UpdateRequestStatusAsync(Guid id, Status status)
        {
            var sql = "[dbo].[spUpdateRequestStatus]";
            var parameters = new
            {
                Id = id,
                NewStatus = (byte)status
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
                throw new Exception("There was a problem with updating request status: ", ex);
            }
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE Status = @PendingStatus AND TransactionType = 2";
            return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { PendingStatus = (byte)Status.Pending });
        }
    }
}
