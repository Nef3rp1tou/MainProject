using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using BankingApi.DTOs;
using BankingApi.Enums;
using BankingApi.IServices;

namespace BankingApi.Services;

public class TransactionService : ITransactionService
{
    private readonly string _secretKey;

    public TransactionService(IConfiguration configuration)
    {
        // Retrieve the secret key from the appsettings.json
        _secretKey = configuration["Security:SecretKey"];
    }
    public async Task<DepositResponseDto> Deposit(DepositRequestDto depositRequestDto)
    {

        if (!ValidateTheHash(depositRequestDto.Hash, depositRequestDto.TransactionId, depositRequestDto.Amount, depositRequestDto.MerchantId))
        {
            return new DepositResponseDto()
            {
                Status = Status.Rejected,
                PaymentUrl = String.Empty
            };
        }

        var isEven = IsEven(depositRequestDto.Amount);
        
        return new DepositResponseDto()
        {
            Status = isEven ? Status.Success : Status.Rejected,
            PaymentUrl = isEven ? $"https://localhost:7038/payment/dummy?transactionId={depositRequestDto.TransactionId}&amount={depositRequestDto.Amount}" : string.Empty
        };
    }

    public async Task<WithdrawResponseDto> Withdraw(WithdrawRequestDto withdrawRequestDto)
    {
        if (!ValidateTheHash(withdrawRequestDto.Hash, withdrawRequestDto.TransactionId, withdrawRequestDto.Amount, 
            withdrawRequestDto.MerchantId, withdrawRequestDto.UserAccountNumber, withdrawRequestDto.UserFullName))
        {
            return new WithdrawResponseDto()
            {
                Status = Status.Rejected
            };
        }

        var isEven = IsEven(withdrawRequestDto.Amount);
        
        return new WithdrawResponseDto()
        {
            Status = isEven ? Status.Success : Status.Rejected,
        };
    }
    
    private bool IsEven(int number)
    {
        return number % 2 == 0;
    }

    private bool ValidateTheHash(string hash, Guid transactionId, int amount, Guid merchantId, string? accountNumber = null, string? fullName = null)
    {
        var rawData = new StringBuilder();
        rawData.Append(amount);
        rawData.Append(merchantId);
        rawData.Append(transactionId);

        if (accountNumber != null)
        {
            rawData.Append(accountNumber);
        }

        if (fullName != null)
        {
            rawData.Append(fullName);
        }

        rawData.Append(_secretKey);

        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData.ToString()));
            var computedHash = Convert.ToHexString(bytes);

            // Compare the computed hash with the provided hash
            return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}