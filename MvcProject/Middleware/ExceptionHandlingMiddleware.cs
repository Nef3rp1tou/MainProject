using Microsoft.Data.SqlClient;
using MvcProject.Enums;
using MvcProject.Utilities;
using System.Net;
using System.Text.Json;

namespace MvcProject.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            ServiceResult errorResponse = null;
            // Default to Internal Server Error.
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case CustomException customEx:
                    response.StatusCode = (int)customEx.StatusCode;
                    string message = GetDefaultMessageForStatusCode(customEx.StatusCode);
                    errorResponse = new ServiceResult(false, message, response.StatusCode);
                    break;

                case InvalidOperationException _:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new ServiceResult(false, exception.Message);
                    break;

                case UnauthorizedAccessException _:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse = new ServiceResult(false, "Unauthorized access.", response.StatusCode);
                    break;

                case SqlException sqlEx:
                    errorResponse = HandleSqlException(sqlEx);
                    response.StatusCode = sqlEx.Number;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new ServiceResult(false, "A server error occurred. Please try again later.", response.StatusCode);
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            return response.WriteAsync(result);
        }

        private static ServiceResult HandleSqlException(SqlException sqlEx)
        {
            return new ServiceResult(false, sqlEx.Message);
        }

        private static string GetDefaultMessageForStatusCode(CustomStatusCode statusCode)
        {
            return statusCode switch
            {
                CustomStatusCode.Success => "Operation completed successfully.",
                CustomStatusCode.AlreadyProcessedTransaction => "Transaction has already been processed.",
                CustomStatusCode.InactiveToken => "Token is inactive.",
                CustomStatusCode.InsufficientBalance => "Insufficient balance for this transaction.",
                CustomStatusCode.InvalidHash => "Invalid hash provided.",
                CustomStatusCode.InvalidToken => "Invalid token provided.",
                CustomStatusCode.TransferLimit => "Transfer limit reached.",
                CustomStatusCode.UserNotFound => "User not found.",
                CustomStatusCode.InvalidAmount => "Invalid amount specified.",
                CustomStatusCode.DuplicatedTransactionId => "Duplicated transaction ID.",
                CustomStatusCode.SessionExpired => "Session has expired.",
                CustomStatusCode.InvalidCurrency => "Invalid currency specified.",
                CustomStatusCode.InvalidRequest => "Invalid request.",
                CustomStatusCode.InvalidIp => "Invalid IP address.",
                CustomStatusCode.GeneralError => "A general error occurred.",
                _ => "An error occurred. Please try again later."
            };
        }
    }
}
