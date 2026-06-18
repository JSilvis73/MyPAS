using Microsoft.EntityFrameworkCore;
using MyPAS.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyPAS.Models
{
    public class Procedure
    {
        [Key]
        public int Id { get; set; }

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

        // Foreign key
        [Required]
        public int PatientId { get; set; }

        // Navigation 
        public Patient Patient { get; set; } = null!;

        [JsonIgnore]
        public List<Payment> Payments { get; set; } = new();
    }
}

