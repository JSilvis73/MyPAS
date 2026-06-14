using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class ProcedureDTO
    {
        public int Id { get; set; }
        public string ProcedureName { get; set; } = string.Empty;
        public DateOnly? ProcedureDate { get; set; }
        public string? CptCode { get; set; }
        public decimal? CptAmount { get; set; }
        public decimal? PatientChargedAmount { get; set; } 
        public int PatientId { get; set; }
    }
}
