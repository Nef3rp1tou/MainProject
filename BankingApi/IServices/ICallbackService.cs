using BankingApi.DTOs;

namespace BankingApi.IServices;

public interface ICallbackService
{
    Task SendCallback(TransactionCallbackDto transactionCallbackDto, bool isDeposit);
}