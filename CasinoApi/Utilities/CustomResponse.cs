using CasinoApi.Enums;

namespace CasinoApi.Utilities
{
    public class CustomResponse
    {
        public CustomStatusCode StatusCode { get; set; }
        public object? Data { get; set; }

        public CustomResponse(CustomStatusCode statusCode, object data) {
            StatusCode = statusCode;
            Data = data;
        }

        public CustomResponse(CustomStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
