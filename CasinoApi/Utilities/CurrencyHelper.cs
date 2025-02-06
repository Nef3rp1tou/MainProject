using CasinoApi.Enums;

namespace CasinoApi.Utilities
{
    public static class CurrencyHelper
    {
        public static Currency ConvertToCurrency(string curr)
        {
            return curr.ToUpper() switch
            {
                "GEL" => Currency.GEL,
                "USD" => Currency.USD,
                "EUR" => Currency.EUR,
                _ => throw new CustomException(CustomStatusCode.InvalidCurrency)
            };
        }
    }
}
