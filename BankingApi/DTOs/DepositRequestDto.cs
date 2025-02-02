namespace BankingApi.DTOs;

public class DepositRequestDto
{
    public int TransactionId { get; set; }
    public int Amount { get; set; }
    public Guid MerchantId { get; set; }
    public string Hash { get; set; } = null!;
}