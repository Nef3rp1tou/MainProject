namespace CasinoApi.DTOs
{
    public class GameTransactionResponseDto
    {
        private int transactionId;

        public GameTransactionResponseDto(int transactionId, decimal currentBalance)
        {
            TransactionId = transactionId.ToString();
            CurrentBalance = currentBalance;
        }

        public string TransactionId { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
