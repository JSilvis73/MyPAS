using MyPAS.Models;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string ServiceName { get; set; }

        public DateOnly ServiceDate { get; set; }

        public string CptCode { get; set; }

        public decimal CptAmount { get; set; }

        public decimal PatientChargedAmount { get; set; }

        // Foreign key
        [Required]
        public int PatientId { get; set; }

        // Navigation 
        public Patient? Patient { get; set; }
    }
}

