using Azure.Core;
using MvcProject.DTOs;
using MvcProject.Interfaces.IServices;
using MvcProject.Models;
using MvcProject.Settings;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

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

        public async Task<ApiResponse> SendDepositRequestAsync(Guid transactionId, decimal amount)
        {
            var dataToHash = $"{amount * 100}{_config.MerchantId}{transactionId}{_config.SecretKey}";
            var hash = GenerateHash(dataToHash);

            var request = new DepositRequestDto
            {
                TransactionId = transactionId,
                Amount = (int)(amount * 100),
                MerchantId = Guid.Parse(_config.MerchantId),
                Hash = hash
            };

            var response = await _httpClient.PostAsJsonAsync($"{_config.BaseUrl}/deposit", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        }
        public async Task<ApiResponse> SendDepositFinishRequestAsync(Guid transactionId, decimal amount)
        {
            var dataToHash = $"{amount * 100}{_config.MerchantId}{transactionId}{_config.SecretKey}";
            var hash = GenerateHash(dataToHash);

            var request = new DepositRequestDto
            {
                TransactionId = transactionId,
                Amount = (int)(amount * 100),
                MerchantId = Guid.Parse(_config.MerchantId),
                Hash = hash
            };

            var response = await _httpClient.PostAsJsonAsync($"{_config.BaseUrl}/deposit-finish", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        }




        public async Task<ApiResponse> SendWithdrawRequestAsync(Guid transactionId, decimal amount, string userId, string accountNumber, string fullName)
        {
            var dataToHash = $"{amount * 100}{_config.MerchantId}{transactionId}{accountNumber}{fullName}{_config.SecretKey}";
            var hash = GenerateHash(dataToHash);

            var request = new
            {
                TransactionID = transactionId,
                Amount = (int)(amount * 100),
                MerchantID = _config.MerchantId,
                UsersAccountNumber = accountNumber,
                UsersFullName = fullName,
                Hash = hash
            };

            var response = await _httpClient.PostAsJsonAsync($"{_config.BaseUrl}/withdraw", request);
            return await response.Content.ReadFromJsonAsync<ApiResponse>();

        }

        public bool ValidateHash(string receivedHash, string expectedData)
        {
            var exceptedHash = GenerateHash(expectedData);
            return string.Equals(exceptedHash, receivedHash, StringComparison.OrdinalIgnoreCase);


        }
        private string GenerateHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
