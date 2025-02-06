namespace CasinoApi.DTOs
{
    public class UserInfoResponseDto
    {
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public string FirstName { get; set; } = "N/A"; 
        public string LastName { get; set; } = "N/A"; 
        public string Email { get; set; } 
        public string CountryCode { get; set; } = "N/A"; 
        public string CountryName { get; set; } = "N/A"; 
        public int? Gender { get; set; } = null; 
        public string Currency { get; set; } 
        public decimal CurrentBalance { get; set; } 
    }
}
