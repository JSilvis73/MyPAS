using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class UpdatePaymentDTO
    {
        [Precision(18, 2)]
        public decimal? Amount { get; set; }

        public string? Method { get; set; }

        public DateOnly? PaymentDate { get; set; }
    }
}
