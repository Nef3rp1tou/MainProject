using System.Security.Cryptography;
using System.Text;

namespace MvcProject.Utilities
{
    public static class HashHelper
    {
        public static string GenerateHash(string input)
        {
            using var sha256 = SHA256.Create();
            return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(input)))
                .Replace("-", "")
                .ToLower();
        }

        public static bool ValidateHash(string receivedHash, string expectedData)
        {
            return string.Equals(GenerateHash(expectedData), receivedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
