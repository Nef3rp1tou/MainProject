using MvcProject.Enums;

namespace MvcProject.Utilities
{
    public class CustomResponse
    {
        public CustomStatusCode StatusCode { get; set; }
        public object? Data { get; set; }
        public string Message { get; set; }

        public CustomResponse(CustomStatusCode statusCode, object? data = null, string? message = null)
        {
            StatusCode = statusCode;
            Data = data;
            Message = message ?? ResponseMessages.GetDefaultMessage(statusCode);
        }
    }
}
