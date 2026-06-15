using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class UpdatePaymentDTO
    {
        public decimal? Amount { get; set; }

        public string? Method { get; set; }

        public DateOnly? PaymentDate { get; set; }
    }
}
