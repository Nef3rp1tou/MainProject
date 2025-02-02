using log4net;
using System.Net;
using System.Text.Json;

namespace BankingApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _logger = LogManager.GetLogger(typeof(ErrorHandlingMiddleware));
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var correlationId = context.Response.Headers["X-Correlation-ID"].ToString();
            _logger.Error($"[{correlationId}] An error occurred: {ex.Message}", ex);

            context.Response.ContentType = "application/json";

            var error = new ErrorResponse
            {
                CorrelationId = correlationId,
                Status = GetStatusCode(ex),
                Message = GetErrorMessage(ex)
            };

            if (_env.IsDevelopment())
            {
                error.DeveloperMessage = new DeveloperError
                {
                    Exception = ex.GetType().Name,
                    StackTrace = ex.StackTrace
                };
            }

            context.Response.StatusCode = error.Status;
            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }

        private static int GetStatusCode(Exception ex) => ex switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            HttpRequestException => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };

        private static string GetErrorMessage(Exception ex) => ex switch
        {
            ArgumentException => "Invalid request parameters",
            InvalidOperationException => "Invalid operation requested",
            KeyNotFoundException => "Requested resource not found",
            UnauthorizedAccessException => "Unauthorized access",
            HttpRequestException => "External service unavailable",
            _ => "An unexpected error occurred"
        };

        private class ErrorResponse
        {
            public string CorrelationId { get; set; }
            public int Status { get; set; }
            public string Message { get; set; }
            public DeveloperError DeveloperMessage { get; set; }
        }

        private class DeveloperError
        {
            public string Exception { get; set; }
            public string StackTrace { get; set; }
        }
    }
}