using CasinoApi.Enums;

namespace CasinoApi.Utilities
{
    public class CustomException : Exception
    {
        public CustomStatusCode StatusCode { get; set; }

        public CustomException(CustomStatusCode statusCode) {
            StatusCode = statusCode;
        }

        public CustomException(CustomStatusCode statusCode, string message) 
        : base(message) 
        {
    
            StatusCode = statusCode;
        }
    }
}
