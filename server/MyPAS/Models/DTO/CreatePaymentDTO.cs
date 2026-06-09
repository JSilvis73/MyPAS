using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class CreatePaymentDTO
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }
        public string? Method { get; set; }

        public string? Notes { get; set; }

        public DateOnly PaymentDate { get; set; }

        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public int ProcedureId { get; set; }

        public Procedure? Procedure { get; set; }
    }
}
