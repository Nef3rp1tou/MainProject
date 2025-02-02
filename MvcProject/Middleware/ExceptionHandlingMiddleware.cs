using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MvcProject.Utilities;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            switch (exception)
            {
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

                case Exception _:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new ServiceResult(false, "A server error occurred. Please try again later.", response.StatusCode);
                    break;
            }

            if(errorResponse == null)
            {
                errorResponse = new ServiceResult(false, "An error occurred. Please try again later.", response.StatusCode);
            }

            var result = JsonSerializer.Serialize(errorResponse);
            return response.WriteAsync(result);
        }

        private static ServiceResult HandleSqlException(SqlException sqlEx)
        {
            return new ServiceResult(false, sqlEx.Message);
        }
    }
}
