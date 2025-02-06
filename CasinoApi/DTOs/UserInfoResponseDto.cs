namespace CasinoApi.DTOs
{
    public class UserInfoResponseDto
    {
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public string FirstName { get; set; } = "Name"; 
        public string LastName { get; set; } = "Surname"; 
        public string Email { get; set; } 
        public string CountryCode { get; set; } = "GEO"; 
        public string CountryName { get; set; } = "Georgia"; 
        public int? Gender { get; set; } = 0; 
        public string Currency { get; set; } 
        public decimal CurrentBalance { get; set; } 
    }
}
