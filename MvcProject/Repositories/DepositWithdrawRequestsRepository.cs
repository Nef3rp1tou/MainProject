using Dapper;
using Microsoft.Data.SqlClient;
using MvcProject.Enums;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Models;
using System.Data;
using log4net;

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

            try
            {
                await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                var id = parameters.Get<int>("@Id");
                return id;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task RejectRequestAsync(DepositWithdrawRequests requests)
        {
            var sql = "[dbo].[RejectRequest]";
            var parameters = new DynamicParameters();

            parameters.Add("@Id", requests.Id, DbType.Int32);
            parameters.Add("@UserId", requests.UserId);
            parameters.Add("@Amount", requests.Amount);
            parameters.Add("@TransactionType", requests.TransactionType);

            try
            {
                await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while rejecting the request.", ex);
            }
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetRequestsByUserIdAsync(string userId)
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE UserId = @UserId";

            try
            {
                return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { UserId = userId });
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<DepositWithdrawRequests> GetRequestByIdAsync(int id)
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE Id = @Id";

            try
            {
                return await _dbConnection.QuerySingleOrDefaultAsync<DepositWithdrawRequests>(sql, new { Id = id });
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE Status = @PendingStatus AND TransactionType = 2";

            try
            {
                return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { PendingStatus = (byte)Status.Pending });
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
