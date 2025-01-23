using BankingApi.DTOs;
using BankingApi.Enums;

namespace BankingApi.Mappers;

public static class ToTransactionCallbackDto
{
    public static TransactionCallbackDto DepositToTransactionCallbackDto(Status status, DepositRequestDto request)
    {
        return new TransactionCallbackDto
        {
            TransactionId = request.TransactionId,
            Amount = request.Amount,
            Status = status
        };
    }

    public static TransactionCallbackDto WithdrawToTransactionCallbackDto(Status status, WithdrawRequestDto request)
    {
        return new TransactionCallbackDto
        {
            TransactionId = request.TransactionId,
            Amount = request.Amount,
            Status = status
        };
    }
}