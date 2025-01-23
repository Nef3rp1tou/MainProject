using BankingApi.DTOs;
using BankingApi.IServices;

namespace BankingApi.Services;

public class CallbackService : ICallbackService
{
    
    private readonly HttpClient _httpClient;
    private readonly string _callbackUrl;
    
    public CallbackService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _callbackUrl = configuration["CallbackSettings:MvcCallbackUrl"] ?? throw new InvalidOperationException();
    }
    
    
    public async Task SendCallback(TransactionCallbackDto transactionCallbackDto)
    {
        var url = _callbackUrl;
        var response = await _httpClient.PostAsJsonAsync(_callbackUrl, transactionCallbackDto);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to send callback. Status Code: {response.StatusCode}");
        }
    }
}