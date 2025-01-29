using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using BankingApi.DTOs;
using BankingApi.Enums;
using BankingApi.IServices;
using BankingApi.Utilities;

namespace BankingApi.Services;

public class TransactionService : ITransactionService
{
    private readonly string _secretKey;
    private readonly string _paymentUrl;
    public TransactionService(IConfiguration configuration)
    {
        _secretKey = configuration["Security:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "Secret key cannot be null");
        _paymentUrl = configuration["Payment:PaymentUrl"] ?? throw new ArgumentNullException(nameof(configuration), "Payment url cannot be null");
    }
    public async Task<DepositResponseDto> Deposit(DepositRequestDto depositRequestDto)
    {
        if (!ValidationHelper.ValidateTheHash(depositRequestDto.Hash, depositRequestDto.TransactionId, depositRequestDto.Amount, depositRequestDto.MerchantId, _secretKey))
        {
            return new DepositResponseDto()
            {
                Status = Status.Rejected,
                PaymentUrl = String.Empty
            };
        }

        var isEven = await Task.Run(() => ValidationHelper.IsEven(depositRequestDto.Amount));

        return new DepositResponseDto()
        {
            Status = isEven ? Status.Success : Status.Rejected,
            PaymentUrl = isEven ? $"{_paymentUrl}{depositRequestDto.TransactionId}&amount={depositRequestDto.Amount}" : string.Empty
        };
    }

    public async Task<WithdrawResponseDto> Withdraw(WithdrawRequestDto withdrawRequestDto)
    {
        if (!ValidationHelper.ValidateTheHash(withdrawRequestDto.Hash, withdrawRequestDto.TransactionId, withdrawRequestDto.Amount, withdrawRequestDto.MerchantId, _secretKey))
        {
            return new WithdrawResponseDto()
            {
                Status = Status.Rejected,
                Amount = withdrawRequestDto.Amount,
                TransactionId = withdrawRequestDto.TransactionId
            };
        }

        var isEven = await Task.Run(() => ValidationHelper.IsEven(withdrawRequestDto.Amount));

        return new WithdrawResponseDto()
        {
            Status = isEven ? Status.Success : Status.Rejected,
            TransactionId = withdrawRequestDto.TransactionId,
            Amount = withdrawRequestDto.Amount
        };
    }
    
   
}