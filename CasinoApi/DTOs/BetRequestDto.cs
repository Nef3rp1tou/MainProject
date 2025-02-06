namespace CasinoApi.DTOs
{
    public class BetRequestDto
    {
        public Guid Token { get; set; } 
        public decimal Amount { get; set; } 
        public string TransactionId { get; set; } 
        public int? BetTypeId { get; set; } 
        public int GameId { get; set; } 
        public int? ProductId { get; set; } 
        public int RoundId { get; set; }
        public string? Hash { get; set; } 
        public string Currency { get; set; } 
        public int? CampaignId { get; set; } 
        public string? CampaignName { get; set; } 
    }
}
