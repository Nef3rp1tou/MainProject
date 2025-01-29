using Microsoft.AspNetCore.Mvc;
using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Settings;
using MvcProject.Utilities;
using System.Net.Http.Json;

namespace MvcProject.Services
{
    public class BankingApiService : IBankingApiService
    {
        private readonly BankingApiConfig _config;
        private readonly HttpClient _httpClient;

        public BankingApiService(BankingApiConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        private object CreateRequest(Guid transactionId, decimal amount)
        {
            var amountInCents = (int)(amount * 100);
            var hashData = $"{amountInCents}{_config.MerchantId}{transactionId}{_config.SecretKey}";

            return new
            {
                TransactionId = transactionId,
                Amount = amountInCents,
                MerchantId = Guid.Parse(_config.MerchantId),
                Hash = HashHelper.GenerateHash(hashData) 
            };
        }

        private async Task<T> PostRequestAsync<T>(string endpoint, object request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.BaseUrl}/{endpoint}", request);

            var result = await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Failed to deserialize response.");

            var statusProperty = typeof(T).GetProperty("Status");
            if (statusProperty != null)
            {
                var status = (Status)(statusProperty.GetValue(result) ?? throw new InvalidOperationException("Status property is null."));
                if (status == Status.Rejected)
                    throw new Exception("Bank rejected the transaction.");
            }
            response.EnsureSuccessStatusCode();
            return result;
        }

        public Task<ApiResponse> SendDepositRequestAsync(Guid transactionId, decimal amount) =>
            PostRequestAsync<ApiResponse>("deposit", CreateRequest(transactionId, amount));

        public Task<DepositFinishRequestDto> SendDepositFinishRequestAsync(Guid transactionId, decimal amount) =>
            PostRequestAsync<DepositFinishRequestDto>("deposit-finish", CreateRequest(transactionId, amount));

        public Task<WithdrawRequestDto> SendWithdrawRequestAsync(Guid transactionId, decimal amount) =>
            PostRequestAsync<WithdrawRequestDto>("withdraw", CreateRequest(transactionId, amount));

        public bool ValidateHash(string receivedHash, string expectedData) =>
            HashHelper.ValidateHash(receivedHash, expectedData);
    }

}
