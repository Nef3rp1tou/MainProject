using CasinoApi.DTOs;
using CasinoApi.Enums;

namespace CasinoApi.Interfaces.IRepositories
{
    public interface IGameTransactionRepository
    {
        Task<GameTransactionResponseDto> PlaceBetAsync(
            Guid token, decimal amount, int transactionId,Currency currency, int gameId, int roundId);
    
        Task<GameTransactionResponseDto> WinAsync(
            Guid token, decimal amount, int transactionId, Currency currency, int gameId, int roundId);

        Task<GameTransactionResponseDto> CancelBetAsync(Guid token, decimal amount, int transactionId,
            Currency currency, int gameId, int roundId, int betTransactionId);

        Task<GameTransactionResponseDto> ChangeWinAsync(Guid token, decimal amount,
            decimal previousAmount, int transactionId, int previousTransactionId, Currency curr, int gameId, int roundId);

    }
}
