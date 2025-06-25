using MyPAS.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPAS.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Method { get; set; }

        public string Notes { get; set; }

        public DateOnly PaymentDate { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int ServiceId { get; set; }

        public Service Service { get; set; }
    }

}



