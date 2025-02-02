using BankingApi.Enums;

namespace BankingApi.DTOs;

public class TransactionCallbackDto
{
    public int TransactionId { get; set; }
    public int Amount { get; set; }
    public Status Status { get; set; }
}