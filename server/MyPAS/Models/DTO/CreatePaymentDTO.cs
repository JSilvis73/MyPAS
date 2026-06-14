using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class CreatePaymentDTO
    {
        [Required]
        [Precision(18,2)]
        public decimal Amount { get; set; }
        [Required]
        public string Method { get; set; } = string.Empty;
        [Required]
        public DateOnly PaymentDate { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int ProcedureId { get; set; }

    }
}
