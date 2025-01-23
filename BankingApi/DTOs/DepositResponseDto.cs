using BankingApi.Enums;

namespace BankingApi.DTOs;

public class DepositResponseDto
{
    public Status Status { get; set; }
    public string PaymentUrl { get; set; } = null!;
}