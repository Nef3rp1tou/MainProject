namespace MvcProject.DTOs
{
    public class DepositFinishRequestDto
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
    }
}
