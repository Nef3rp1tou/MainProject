using MvcProject.Enums;
using MvcProject.Utilities;
using System.Text.Json;

namespace MvcProject.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                await HandleExceptionAsync(context, ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context, CustomStatusCode.GeneralError, "An unexpected error occurred.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, CustomStatusCode statusCode, string message)
        {
            var response = new CustomResponse(statusCode, message: message);
            var jsonResponse = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
