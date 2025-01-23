using Microsoft.AspNetCore.Identity;
using MvcProject.Enums;
using System;

namespace MvcProject.Models
{
    public class Transactions
    {
        public Guid Id { get; set; } 
        public string UserId { get; set; } 
        public decimal Amount { get; set; } 
        public Status Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public virtual IdentityUser User { get; set; }

    }
}
