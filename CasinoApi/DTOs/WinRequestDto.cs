namespace CasinoApi.DTOs
{
    public class WinRequestDto
    {
        public Guid Token { get; set; } 
        public decimal Amount { get; set; } 
        public string TransactionId { get; set; } 
        public int GameId { get; set; } 
        public int RoundId { get; set; } 

        public int? WinTypeId { get; set; }
        public int? ProductId { get; set; }
        public string? Hash { get; set; }
        public string Currency { get; set; }
        public int? CampaignId { get; set; }
        public string? CampaignName { get; set; }
    }

}
