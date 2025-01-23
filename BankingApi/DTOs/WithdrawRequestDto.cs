namespace BankingApi.DTOs;

public class WithdrawRequestDto
{
    public Guid TransactionId { get; set; }
    public int Amount { get; set; }
    public Guid MerchantId { get; set; }
    public string UserAccountNumber { get; set; } = null!;
    public string UserFullName { get; set; } = null!;
    public string Hash { get; set; } = null!;
}