using CasinoApi.Enums;
using Microsoft.AspNetCore.Identity;
namespace CasinoApi.Models
{
    public class Transactions
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; } 
        public Status Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public int? GameId { get; set; }
        public int? RoundId { get; set; }
        public virtual IdentityUser User { get; set; }

    }
}
