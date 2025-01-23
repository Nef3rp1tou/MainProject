using MvcProject.Enums;

namespace MvcProject.Models
{
    public class ApiResponse
    {
        public Status Status { get; set; } 
        public string Message { get; set; } 
        public string PaymentUrl { get; set; } 
    } 
}
