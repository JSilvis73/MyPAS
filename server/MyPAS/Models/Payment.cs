using MyPAS.Models;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public string? Method { get; set; }

        public string? Notes { get; set; }

        public DateOnly PaymentDate { get; set; }

        public int PatientId { get; set; }

        [Required]
        public Patient? Patient { get; set; }

        public int ProcedureId { get; set; }

        public Procedure? Procedure { get; set; }
    }

}



