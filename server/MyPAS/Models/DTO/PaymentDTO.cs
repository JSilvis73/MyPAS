using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        
        public decimal Amount { get; set; }

        public string Method { get; set; } = string.Empty;

        public DateOnly PaymentDate { get; set; }

        public int PatientId { get; set; }

        public int ProcedureId { get; set; }

    }
}
