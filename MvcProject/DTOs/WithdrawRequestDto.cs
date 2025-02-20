﻿using MvcProject.Enums;

namespace MvcProject.DTOs
{
    public class WithdrawRequestDto
    {
        public int TransactionId { get; set; }
        public int Amount { get; set; }
        public Guid MerchantId { get; set; }
        public string Hash { get; set; } = null!;
        public Status Status { get; set; }
    }
}
