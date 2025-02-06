namespace MvcProject.Models
{
    public class UserToken
    {
        public int Id { get; set; }                      
        public string UserId { get; set; }               
        public Guid PublicToken { get; set; }           
        public Guid? PrivateToken { get; set; }        
        public bool IsPublicTokenValid { get; set; }    
        public bool IsPrivateTokenValid { get; set; }    
        public DateTime CreatedAt { get; set; }         
        public DateTime UpdatedAt { get; set; }         
    }

}
