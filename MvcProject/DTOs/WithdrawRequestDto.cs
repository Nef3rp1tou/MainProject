namespace MvcProject.DTOs
{
    public class WithdrawRequestDto
    {
        public Guid TransactionId { get; set; }
        public int Amount { get; set; }
        public Guid MerchantId { get; set; }
        public string Hash { get; set; } = null!;
    }
}
