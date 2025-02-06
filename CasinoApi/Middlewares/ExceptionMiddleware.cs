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



            CustomResponse errorResponse = null;

            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case CustomException customEx:
                    response.StatusCode = (int)customEx.StatusCode;
                    errorResponse = new CustomResponse((CustomStatusCode)response.StatusCode);
                    break;

                case SqlException sqlEx:
                    errorResponse = HandleSqlException(sqlEx);
                    response.StatusCode = sqlEx.Number;
                    break;

                default:
                    errorResponse = new CustomResponse(CustomStatusCode.GeneralError);
                    break;
            }

            
            if (errorResponse.Data == null)
            {
                return response.WriteAsync(JsonSerializer.Serialize(new
                {
                    StatusCode = errorResponse.StatusCode
                }));
            }
            return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }


        private static CustomResponse HandleSqlException(SqlException sqlEx)
        {
            return new CustomResponse((CustomStatusCode)sqlEx.Number);
        }

    }

}
