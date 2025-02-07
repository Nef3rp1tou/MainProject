namespace MvcProject.DTOs
{
    public class AdminActionResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public AdminActionResponseDto(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
