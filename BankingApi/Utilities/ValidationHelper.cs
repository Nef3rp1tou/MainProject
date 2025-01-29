using System.Security.Cryptography;
using System.Text;

namespace BankingApi.Utilities
{
    public static class ValidationHelper
    {
        public static bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        public static bool ValidateTheHash(string hash, Guid transactionId, int amount, Guid merchantId, string secretKey)
        {
            var rawData = $"{amount}{merchantId}{transactionId}{secretKey}";

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(rawData);
            var hash2 = sha256.ComputeHash(bytes);
            var computedHash = BitConverter.ToString(hash2).Replace("-", "").ToLower();

            return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);

        }
    }
}
