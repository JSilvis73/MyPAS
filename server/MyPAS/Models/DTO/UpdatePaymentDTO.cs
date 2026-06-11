namespace MyPAS.Models.DTO
{
    public class UpdatePaymentDTO
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string? Method { get; set; }

        public DateOnly PaymentDate { get; set; }

        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public int ProcedureId { get; set; }

        public Procedure? Procedure { get; set; }
    }
}
