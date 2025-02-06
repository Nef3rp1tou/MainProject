namespace CasinoApi.DTOs
{
    public class GetBalanceResponseDto
    {
        private decimal balance;

        public GetBalanceResponseDto(decimal balance)
        {
            CurrentBalance = balance;
        }

        public decimal CurrentBalance { get; set; }
    }
}
