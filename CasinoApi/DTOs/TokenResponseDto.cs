namespace CasinoApi.DTOs
{
    public class TokenResponseDto
    {
        public TokenResponseDto(Guid privateToken)
        {
            PrivateToken = privateToken;
        }

        public Guid PrivateToken { get; set; }

    }
}
