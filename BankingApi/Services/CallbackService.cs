using BankingApi.DTOs;
using BankingApi.IServices;

namespace BankingApi.Services;

public class CallbackService : ICallbackService
{
    
    private readonly HttpClient _httpClient;
    private readonly string _callbackDepositUrl; 
    private readonly string _callbackWithdrawUrl;

    public CallbackService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _callbackDepositUrl = configuration["CallbackSettings:MvcCallbackDepositUrl"] ?? throw new InvalidOperationException();
        _callbackWithdrawUrl = configuration["CallbackSettings:MvcCallbackWithdrawUrl"] ?? throw new InvalidOperationException();

    }
    
    
    public async Task SendCallback(TransactionCallbackDto transactionCallbackDto, bool isDeposit)
    {
        var response = isDeposit ? await _httpClient.PostAsJsonAsync(_callbackDepositUrl, transactionCallbackDto)
            : await _httpClient.PostAsJsonAsync(_callbackWithdrawUrl, transactionCallbackDto);
        

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to send callback. Status Code: {response.StatusCode}");
        }
    }

}