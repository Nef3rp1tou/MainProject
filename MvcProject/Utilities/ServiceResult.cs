namespace MvcProject.Utilities
{
    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public object? Data { get; }

        public ServiceResult(bool success, string message, object? data = null)
        {
            IsSuccess = success;
            Message = message;
            Data = data;
        }

        public static ServiceResult Success(string message, object? data = null) =>
            new ServiceResult(true, message, data);

        public static ServiceResult Failure(string message, object? data = null) =>
            new ServiceResult(false, message, data);
    }
}