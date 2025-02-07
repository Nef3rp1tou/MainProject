using MvcProject.Enums;

namespace MvcProject.Utilities
{
    public class CustomException : Exception
    {
        public CustomStatusCode StatusCode { get; set; }

        public CustomException(CustomStatusCode statusCode)
            : base(ResponseMessages.GetDefaultMessage(statusCode))
        {
            StatusCode = statusCode;
        }

        public CustomException(CustomStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
