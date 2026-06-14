using Microsoft.EntityFrameworkCore;
using MyPAS.Models;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        [Required]
        public string Method { get; set; } = string.Empty;
        [Required]
        public DateOnly PaymentDate { get; set; }
        [Required]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }
        [Required]
        public int ProcedureId { get; set; }

        public Procedure? Procedure { get; set; }
    }

}



