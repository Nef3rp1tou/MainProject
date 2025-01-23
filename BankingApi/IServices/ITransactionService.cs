using BankingApi.DTOs;

namespace BankingApi.IServices;

public interface ITransactionService
{
    Task<DepositResponseDto> Deposit(DepositRequestDto depositRequestDto);
    Task<WithdrawResponseDto> Withdraw(WithdrawRequestDto withdrawRequestDto);
}