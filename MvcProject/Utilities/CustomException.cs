using MvcProject.Enums;

namespace MvcProject.Utilities
{
    public class CustomException : Exception
    {
        private CustomStatusCode insufficientBalance;

        public CustomStatusCode StatusCode { get; }

        public CustomException(CustomStatusCode statusCode, string message)
       : base(message)
        {
            StatusCode = statusCode;
        }

        public CustomException(CustomStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

    }
}
