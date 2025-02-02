using Microsoft.AspNetCore.Identity;
using MvcProject.Enums;

namespace MvcProject.Models
{
    public class DepositWithdrawRequests
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
