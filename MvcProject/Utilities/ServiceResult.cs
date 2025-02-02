namespace MvcProject.Utilities
{
    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public int StatusCode { get; }
        public object? Data { get; }

        private ServiceResult(bool success, string message, int? statusCode = null, object? data = null)
        {
            IsSuccess = success;
            Message = message;
            StatusCode = statusCode ?? (success ? 200 : 400);
            Data = data;
        }

        public static ServiceResult Success(string message, int? statusCode = null, object? data = null) =>
            new ServiceResult(true, message, statusCode, data);

     
        public static ServiceResult Failure(string message, int? statusCode = null, object? data = null) =>
            new ServiceResult(false, message, statusCode, data);
    }
}
