namespace CasinoApi.DTOs
{
    public class CancelBetRequestDto
    {
        public Guid Token { get; set; } 
        public Decimal Amount { get; set; } 
        public string TransactionId { get; set; } 
        public int? BetTypeId { get; set; } 
        public int GameId { get; set; } 
        public int? ProductId { get; set; } 
        public int RoundId { get; set; } 
        public string? Hash { get; set; }
        public string Currency { get; set; } 
        public string BetTransactionId { get; set; } 
    }

}
