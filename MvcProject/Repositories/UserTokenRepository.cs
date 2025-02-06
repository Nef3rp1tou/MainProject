using Dapper;
using MvcProject.Interfaces.IRepositories;
using System.Data;
using System.Threading.Tasks;

namespace MvcProject.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserTokenRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> GeneratePublicToken(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@NewPublicToken", dbType: DbType.Guid, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("GeneratePublicToken", parameters, commandType: CommandType.StoredProcedure);

            var newPublicToken = parameters.Get<Guid>("@NewPublicToken");


            return newPublicToken;
        }

    }

}
