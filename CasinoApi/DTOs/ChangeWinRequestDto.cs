namespace CasinoApi.DTOs
{
    public class ChangeWinRequestDto
    {
        public Guid Token { get; set; } 
        public decimal Amount { get; set; } 
        public decimal PreviousAmount { get; set; } 
        public string TransactionId { get; set; } 
        public string PreviousTransactionId { get; set; } 
        public int? ChangeWinTypeId { get; set; } 
        public int GameId { get; set; } 
        public int? ProductId { get; set; } 
        public int RoundId { get; set; } 
        public string? Hash { get; set; } 
        public string Currency { get; set; } 
    }
}
