﻿using MvcProject.Enums;

namespace MvcProject.Models
{
    public class CallbackRequestModel
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; } // Success/Rejected
    }
}
