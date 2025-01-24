using BankingApi.DTOs;
using BankingApi.Enums;
using BankingApi.IServices;
using BankingApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService, ICallbackService callbackService) : ControllerBase
{
    
    [HttpPost]
    [Route("deposit")]
    public async Task<ActionResult<DepositResponseDto>> Deposit([FromBody] DepositRequestDto depositRequestDto)
    {
        var response = await transactionService.Deposit(depositRequestDto);
        
        return Ok(response);
    }


    [HttpPost]
    [Route("deposit-finish")]
    public async Task<ActionResult<DepositFinishRequestDto>> DepositFinish([FromBody] DepositRequestDto depositRequestDto)
    {
        var transactionCallbackDto = ToTransactionCallbackDto.DepositToTransactionCallbackDto(Status.Success, depositRequestDto);

        await callbackService.SendCallback(transactionCallbackDto, true);

        var response = new DepositFinishRequestDto
        {
            TransactionId = transactionCallbackDto.TransactionId,
            Amount = transactionCallbackDto.Amount,
            Status = transactionCallbackDto.Status
        };

        return Ok(response);
    }

    [HttpPost]
    [Route("withdraw")]
    public async Task<ActionResult<WithdrawResponseDto>> Withdraw([FromBody] WithdrawRequestDto requestDto)
    {
        var response = await transactionService.Withdraw(requestDto);
        
        if(response.Status == Status.Success)
        {
            await callbackService.SendCallback(ToTransactionCallbackDto.WithdrawToTransactionCallbackDto(Status.Success, requestDto), false);
        }
        
        return Ok(response);
    }
    
    
}