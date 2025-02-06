using CasinoApi.Enums;
using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Utilities;
using Dapper;
using System.Data;

namespace CasinoApi.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IDbConnection _dbConnection;

        public TokenRepository(IDbConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> CreatePrivateTokenAsync(Guid publicToken)
        {

            var sql = "[Dbo].[GeneratePrivateToken]";
            var parameters = new DynamicParameters();
            parameters.Add("@PublicToken", publicToken);
            parameters.Add("@NewPrivateToken", dbType: DbType.Guid, direction: ParameterDirection.Output);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            int statusCodeValue = parameters.Get<int>("@StatusCode");

            if (statusCodeValue != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCodeValue);
            }

            return parameters.Get<Guid>("@NewPrivateToken");

        }
    }
}
