using MyPAS.Models;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        [Required]
        public string ProcedureName { get; set; } = string.Empty;

        [Required]
        public DateOnly ProcedureDate { get; set; }

        public string CptCode { get; set; } = string.Empty;

        public decimal CptAmount { get; set; }

        [Required]
        public decimal PatientChargedAmount { get; set; } = 0;

        // Foreign key
        [Required]
        public int PatientId { get; set; }

        // Navigation 
        public Patient? Patient { get; set; } 
    }
}

