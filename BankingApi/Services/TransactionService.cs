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
        _secretKey = configuration["Security:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "Secret key cannot be null");
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

        var isEven = await Task.Run(() => IsEven(depositRequestDto.Amount));

        return new DepositResponseDto()
        {
            Status = isEven ? Status.Success : Status.Rejected,
            PaymentUrl = isEven ? $"https://localhost:7038/payment/dummy?transactionId={depositRequestDto.TransactionId}&amount={depositRequestDto.Amount}" : string.Empty
        };
    }

    public async Task<WithdrawResponseDto> Withdraw(WithdrawRequestDto withdrawRequestDto)
    {
        if (!ValidateTheHash(withdrawRequestDto.Hash, withdrawRequestDto.TransactionId, withdrawRequestDto.Amount, withdrawRequestDto.MerchantId))
        {
            return new WithdrawResponseDto()
            {
                Status = Status.Rejected,
                Amount = withdrawRequestDto.Amount,
                TransactionId = withdrawRequestDto.TransactionId
            };
        }

        var isEven = await Task.Run(() => IsEven(withdrawRequestDto.Amount));

        return new WithdrawResponseDto()
        {
            Status = isEven ? Status.Success : Status.Rejected,
            TransactionId = withdrawRequestDto.TransactionId,
            Amount = withdrawRequestDto.Amount
        };
    }
    
    private bool IsEven(int number)
    {
        return number % 2 == 0;
    }

    private bool ValidateTheHash(string hash, Guid transactionId, int amount, Guid merchantId)
    {
        var rawData = $"{amount}{merchantId}{transactionId}{_secretKey}";

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(rawData);
        var hash2 = sha256.ComputeHash(bytes);
        var computedHash = BitConverter.ToString(hash2).Replace("-", "").ToLower();

       return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
        
    }
}