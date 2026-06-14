using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class UpdateProcedureDTO
    {
        public string? ProcedureName { get; set; }
        public DateOnly? ProcedureDate { get; set; }
        public string? CptCode { get; set; }
        [Precision(18, 2)]
        public decimal? CptAmount { get; set; }
        [Precision(18, 2)]
        public decimal? PatientChargedAmount { get; set; }
    }
}
