using MvcProject.Enums;

namespace MvcProject.DTOs
{
    public class TransactionDto
    {
        public int TransactionRequestId { get; set; }
        public string UserId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
    }
}
