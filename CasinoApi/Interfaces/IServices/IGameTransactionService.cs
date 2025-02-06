using CasinoApi.DTOs;

namespace CasinoApi.Interfaces.IServices
{
    public interface IGameTransactionService
    {
        Task<GameTransactionResponseDto> PlaceBet(BetRequestDto request);
        Task<GameTransactionResponseDto> WinAsync(WinRequestDto request);
        Task<GameTransactionResponseDto> CancelBetAsync(CancelBetRequestDto request);
        Task<GameTransactionResponseDto> ChangeWinAsync(ChangeWinRequestDto request);

    }
}
