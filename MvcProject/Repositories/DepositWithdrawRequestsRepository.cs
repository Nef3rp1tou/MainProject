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

        public async Task RegisterDepositRequestAsync(DepositWithdrawRequests request)
        {
            var sql = "[dbo].[spRegisterDeposit]";

            var parameters = new DynamicParameters();

            parameters.Add("@Id", request.Id);
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Amount", request.Amount);
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
                throw new Exception("An error occurred while creating the deposit request.", ex);
            }

            }

        public async Task RegisterWithdrawRequestAsync(DepositWithdrawRequests request)
        {
            var sql = "[dbo].[spRegisterWithdraw]";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id);
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Amount", request.Amount);
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
                throw new Exception("An error occurred while creating the withdraw request.", ex);
            }
        }

        public async Task RejectRequestAsync(DepositWithdrawRequests requests)
        {
            var sql = "[dbo].[spRejectRequest]";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", requests.Id);
            parameters.Add("@UserId", requests.UserId);
            parameters.Add("@Amount", requests.Amount);
            parameters.Add("@TransactionType", requests.TransactionType);

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
                throw new Exception("An error occurred while rejecting the request.", ex);
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
            return await _dbConnection.QuerySingleOrDefaultAsync<DepositWithdrawRequests>(sql, new { Id = id }); ;
        }


        public async Task<IEnumerable<DepositWithdrawRequests>> GetPendingRequestsAsync()
        {
            var sql = "SELECT * FROM DepositWithdrawRequests WHERE Status = @PendingStatus AND TransactionType = 2";
            return await _dbConnection.QueryAsync<DepositWithdrawRequests>(sql, new { PendingStatus = (byte)Status.Pending });
        }
    }
}
