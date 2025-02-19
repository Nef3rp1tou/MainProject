﻿using CasinoApi.DTOs;
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
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var userInfo = await _dbConnection.QueryFirstOrDefaultAsync<UserInfoResponseDto>(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

            return userInfo;
        }
    
    }
}
