﻿using Microsoft.AspNetCore.Identity;
using MvcProject.Enums;

namespace MvcProject.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }                 
        public string UserId { get; set; }           
        public decimal CurrentBalance { get; set; }  
        public Currency Currency { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
