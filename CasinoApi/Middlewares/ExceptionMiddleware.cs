using CasinoApi.Enums;
using CasinoApi.Utilities;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace CasinoApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.OK;

            CustomStatusCode statusCode = CustomStatusCode.GeneralError; 

            switch (exception)
            {
                case CustomException customEx:
                    statusCode = customEx.StatusCode;
                    break;

                case SqlException sqlEx:
                    statusCode = (CustomStatusCode)sqlEx.Number;
                    break;
            }

            var errorResponse = new { StatusCode = (int)statusCode }; 

            return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
