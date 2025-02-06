using CasinoApi.DTOs;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Utilities;
using Dapper;
using System.Data;

namespace CasinoApi.Repositories
{
    public class GameTransactionRepository : IGameTransactionRepository
    {
        private readonly IDbConnection _dbConnection;

        public GameTransactionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<GameTransactionResponseDto> PlaceBetAsync(
      Guid token, decimal amount, int transactionId, Currency curr, int gameId, int roundId)
        {
            var sql = "[Dbo].[PlaceBet]";
            var parameters = new DynamicParameters();
            parameters.Add("@PrivateToken", token);
            parameters.Add("@Amount", amount);
            parameters.Add("@TransactionId", transactionId);
            parameters.Add("@TransactionType", TransactionType.Bet);
            parameters.Add("@Currency", curr);
            parameters.Add("@GameId", gameId);
            parameters.Add("@RoundId", roundId);
            parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

            var currentBalance = parameters.Get<decimal>("@CurrentBalance");


            return new GameTransactionResponseDto(transactionId, currentBalance);


        }

        public async Task<GameTransactionResponseDto> WinAsync(
        Guid token, decimal amount, int transactionId, Currency curr, int gameId, int roundId)
        {
            var sql = "[Dbo].[Win]";
            var parameters = new DynamicParameters();
            parameters.Add("@PrivateToken", token);
            parameters.Add("@Amount", amount);
            parameters.Add("@TransactionId", transactionId);
            parameters.Add("@TransactionType", TransactionType.Win);
            parameters.Add("@Currency", curr);
            parameters.Add("@GameId", gameId);
            parameters.Add("@RoundId", roundId);
            parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

            var currentBalance = parameters.Get<decimal>("@CurrentBalance");


            return new GameTransactionResponseDto(transactionId, currentBalance);


        }

        public async Task<GameTransactionResponseDto> CancelBetAsync(
       Guid token, decimal amount, int transactionId, Currency curr, int gameId, int roundId, int betTransactionId)
        {
            var sql = "[Dbo].[CancelBet]";
            var parameters = new DynamicParameters();
            parameters.Add("@PrivateToken", token);
            parameters.Add("@Amount", amount);
            parameters.Add("@TransactionId", transactionId);
            parameters.Add("@TransactionType", TransactionType.CancelBet);
            parameters.Add("@Currency", curr);
            parameters.Add("@GameId", gameId);
            parameters.Add("@RoundId", roundId);
            parameters.Add("@BetTransactionId", betTransactionId);
            parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }

            var currentBalance = parameters.Get<decimal>("@CurrentBalance");


            return new GameTransactionResponseDto(transactionId, currentBalance);


        }

        public async Task<GameTransactionResponseDto> ChangeWinAsync(Guid token, decimal amount, decimal previousAmount, int transactionId, int previousTransactionId, Currency curr, int gameId, int roundId)
        {
            var sql = "[Dbo].[ChangeWin]";
            var parameters = new DynamicParameters();
            parameters.Add("@PrivateToken", token);
            parameters.Add("@Amount", amount);
            parameters.Add("@PreviousAmount", previousAmount);
            parameters.Add("@TransactionId", transactionId);
            parameters.Add("@PreviousTransactionId", previousTransactionId);
            parameters.Add("@TransactionType", TransactionType.ChangeWin);
            parameters.Add("@Currency", curr);
            parameters.Add("@GameId", gameId);
            parameters.Add("@RoundId", roundId);
            parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            var statusCode = parameters.Get<int>("@StatusCode");
            if (statusCode != (int)CustomStatusCode.Success)
            {
                throw new CustomException((CustomStatusCode)statusCode);
            }
            var currentBalance = parameters.Get<decimal>("@CurrentBalance");
            return new GameTransactionResponseDto(transactionId, currentBalance);
        }

    }
}
