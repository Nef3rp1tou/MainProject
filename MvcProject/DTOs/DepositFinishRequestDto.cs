using MvcProject.Enums;

namespace MvcProject.DTOs
{
    public class DepositFinishRequestDto
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
    }
}
