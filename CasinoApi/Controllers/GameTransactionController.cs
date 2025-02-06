using CasinoApi.DTOs;
using CasinoApi.Enums;
using CasinoApi.Interfaces.IServices;
using CasinoApi.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CasinoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameTransactionController : Controller
    {
        private readonly IGameTransactionService _gameTransactionService;

        public GameTransactionController(IGameTransactionService gameTransactionService)
        {
            _gameTransactionService = gameTransactionService;
        }

        [HttpPost("bet")]
        public async Task<IActionResult> PlaceBet([FromBody] BetRequestDto request)
        {
            var result = await _gameTransactionService.PlaceBet(request);
            return Ok(new CustomResponse(CustomStatusCode.Success, result));
        }

        [HttpPost("win")]
        public async Task<IActionResult> WinAsync([FromBody] WinRequestDto request)
        {
            var result = await _gameTransactionService.WinAsync(request);
            return Ok(new CustomResponse(CustomStatusCode.Success, result));
        }

        [HttpPost("cancelbet")]
        public async Task<IActionResult> CancelBetAsync([FromBody] CancelBetRequestDto request)
        {
            var result = await _gameTransactionService.CancelBetAsync(request);
            return Ok(new CustomResponse(CustomStatusCode.Success, result));
        }

        [HttpPost("changewin")]
        public async Task<IActionResult> ChangeWinAsync([FromBody] ChangeWinRequestDto request)
        {
            var result = await _gameTransactionService.ChangeWinAsync(request);
            return Ok(new CustomResponse(CustomStatusCode.Success, result));
        }
    }
}
