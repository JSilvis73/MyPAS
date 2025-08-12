using MyPAS.Models;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string ServiceName { get; set; } = string.Empty;

        [Required]
        public DateOnly ServiceDate { get; set; }

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

