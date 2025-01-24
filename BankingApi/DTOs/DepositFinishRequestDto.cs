using BankingApi.Enums;

namespace BankingApi.DTOs
{
    public class DepositFinishRequestDto
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
    }
}
