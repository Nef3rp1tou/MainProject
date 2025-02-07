using Dapper;
using Microsoft.Data.SqlClient;
using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;
using MvcProject.Utilities;

namespace MvcProject.Repositories
{
    public class DepositWithdrawRequestsRepository(IDbConnection dbConnection) : IDepositWithdrawRequestsRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<int> RegisterTransactionAsync(DepositWithdrawRequests request)
        {
            var sql = "[dbo].[RegisterTransaction]";
            var parameters = new DynamicParameters();

            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Amount", request.Amount);
            parameters.Add("@TransactionType", request.TransactionType);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

            return parameters.Get<int>("@Id");
        }

        public async Task RejectRequestAsync(DepositWithdrawRequests request)
        {
            var sql = "[dbo].[RejectRequest]";
            var parameters = new DynamicParameters();

            parameters.Add("@Id", request.Id);
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Amount", request.Amount);
            parameters.Add("@TransactionType", request.TransactionType);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId)
        {
            var sql = $"SELECT * FROM {nameof(DepositWithdrawRequests)} WHERE {nameof(DepositWithdrawRequests.UserId)} = @UserId";

            return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { UserId = userId });
        }

        public async Task<DepositWithdrawRequests?> GetRequestByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {nameof(DepositWithdrawRequests)} WHERE {nameof(DepositWithdrawRequests.Id)} = @Id";

            var result = await _dbConnection.QuerySingleOrDefaultAsync<DepositWithdrawRequests>(sql, new { Id = id });

            if (result == null)
            {
                throw new CustomException(CustomStatusCode.InvalidRequest, "Request not found.");
            }

            return result;
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            var sql = $"SELECT * FROM {nameof(DepositWithdrawRequests)} WHERE {nameof(DepositWithdrawRequests.Status)} = @PendingStatus AND {nameof(DepositWithdrawRequests.TransactionType)} = @TransactionType";

            return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new
            {
                PendingStatus = (byte)Status.Pending,
                TransactionType = (byte)TransactionType.Withdraw
            });
        }
    }
}
