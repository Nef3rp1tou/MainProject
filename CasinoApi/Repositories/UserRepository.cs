using CasinoApi.DTOs;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Utilities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CasinoApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<GetBalanceResponseDto> GetBalanceAsync(Guid token, Currency currency)
        {
            var sql = "[Dbo].[GetBalance]";
            var parameters = new DynamicParameters();

            parameters.Add("@PrivateToken", token);
            parameters.Add("@Currency", currency);
            parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

            var currentBalance = parameters.Get<decimal>("@CurrentBalance");

            return new GetBalanceResponseDto(currentBalance);
        }
        public async Task<UserInfoResponseDto> GetPlayerInfoAsync(Guid token)
        {
            var sql = "[Dbo].[GetPlayerInfo]";
            var parameters = new DynamicParameters();

            parameters.Add("@PrivateToken", token);
            parameters.Add("@UserId", dbType: DbType.String, direction: ParameterDirection.Output, size: 450);
            parameters.Add("@UserName", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
            parameters.Add("@Email", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
            parameters.Add("@Currency", dbType: DbType.Int16, direction: ParameterDirection.Output);
            parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

            Currency currency = (Currency)parameters.Get<short>("@Currency");

            return new UserInfoResponseDto
            {
                UserId = parameters.Get<string>("@UserId"),
                UserName = parameters.Get<string>("@UserName"),
                Email = parameters.Get<string>("@Email"),
                Currency =  currency.ToString(),
                CurrentBalance = parameters.Get<decimal>("@CurrentBalance")
            };
        }
    

        public Task<UserInfoResponseDto> GetUserInfoAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
