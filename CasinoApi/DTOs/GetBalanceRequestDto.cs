namespace CasinoApi.DTOs
{
    public class GetBalanceRequestDto
    {
        public Guid Token { get; set; } 
        public int? GameId { get; set; } 
        public int? ProductId { get; set; } 
        public string? Hash { get; set; } 
        public string Currency { get; set; }
    }
}
