using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class CreateProcedureDTO
    {
        [Required]
        public string ProcedureName { get; set; } = string.Empty;

        [Required]
        public DateOnly ProcedureDate { get; set; }

        public string? CptCode { get; set; }

        [Precision(18, 2)]
        public decimal? CptAmount { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal PatientChargedAmount { get; set; }

        [Required]
        public int PatientId { get; set; }
    }
}
