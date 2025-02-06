using MvcProject.Enums;

namespace MvcProject.DTOs
{
    public class TokenValidationResultDto
    {
        public CustomStatusCode StatusCode { get; set; }
        public Guid? PrivateToken { get; set; }
    }
}
