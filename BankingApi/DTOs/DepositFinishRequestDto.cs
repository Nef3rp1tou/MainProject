using BankingApi.Enums;

namespace BankingApi.DTOs
{
    public class DepositFinishRequestDto
    {
        public Guid TransactionId { get; set; }
        public int Amount { get; set; }
        public Status Status { get; set; }
    }
}
