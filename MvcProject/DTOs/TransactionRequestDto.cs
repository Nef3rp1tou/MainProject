using System.ComponentModel.DataAnnotations;
namespace MvcProject.DTOs
{
    public class TransactionRequestDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }

}
