using CasinoApi.DTOs;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Interfaces.IServices;
using CasinoApi.Utilities;

namespace CasinoApi.Services
{
    public class GameTransactionService : IGameTransactionService
    {
        private readonly IGameTransactionRepository _gameTransactionRepository;

        public GameTransactionService(IGameTransactionRepository gameTransactionRepository)
        {
            _gameTransactionRepository = gameTransactionRepository;
        }

        public async Task<GameTransactionResponseDto> PlaceBet(BetRequestDto request)
        {
            Currency currency = CurrencyHelper.ConvertToCurrency(request.Currency);

            var result = await _gameTransactionRepository.PlaceBetAsync(request.Token, request.Amount, int.Parse(request.TransactionId), currency,
                                                            request.GameId, request.RoundId);

            return result;
        }

        public async Task<GameTransactionResponseDto> WinAsync(WinRequestDto request)
        {
            Currency currency = CurrencyHelper.ConvertToCurrency(request.Currency);

            var result = await _gameTransactionRepository.WinAsync(request.Token, request.Amount, int.Parse(request.TransactionId), currency,
                                                      request.GameId, request.RoundId);

            return result;
        }

        public async Task<GameTransactionResponseDto> CancelBetAsync(CancelBetRequestDto request)
        {
            Currency currency = CurrencyHelper.ConvertToCurrency(request.Currency);
            var result = await _gameTransactionRepository.CancelBetAsync(request.Token, request.Amount, int.Parse(request.TransactionId), currency,
                                                      request.GameId, request.RoundId, int.Parse(request.BetTransactionId));
            return result;
        }

        public async Task<GameTransactionResponseDto> ChangeWinAsync(ChangeWinRequestDto request)
        {
            Currency currency = CurrencyHelper.ConvertToCurrency(request.Currency);
            var result = await _gameTransactionRepository.ChangeWinAsync(request.Token, request.Amount,
                request.PreviousAmount, int.Parse(request.TransactionId), int.Parse(request.PreviousTransactionId), currency, request.GameId, request.RoundId );
            return result;
        }
       
    }
}
