using MyPAS.Data;
using MyPAS.Interfaces;
using MyPAS.Models;
using MyPAS.Models.DTO;

namespace MyPAS.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly MyPASContext _context;
        private readonly ILogger _logger;


        public PaymentServices(MyPASContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }


        public async Task<PaymentDTO?> CreatePaymentWithCreatePaymentDTO(CreatePaymentDTO createPaymentDTO)
        {
            var patientToAddPayment = await _context.Patients.FindAsync(createPaymentDTO.PatientId);
            var procedureToAddPayment = await _context.Procedures.FindAsync(createPaymentDTO.ProcedureId);

            if (patientToAddPayment != null && procedureToAddPayment != null)
            {

                Payment payment = new Payment
                {
                    PatientId = patientToAddPayment.Id,
                    Patient = createPaymentDTO.Patient,
                    ProcedureId = createPaymentDTO.ProcedureId,
                    Procedure = createPaymentDTO.Procedure,
                    PaymentDate = createPaymentDTO.PaymentDate,
                    Amount = createPaymentDTO.Amount,
                    Method = createPaymentDTO.Method,
                    Notes = createPaymentDTO.Notes,
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                return new PaymentDTO
                {
                    Id = payment.Id,
                    PatientId = payment.PatientId, 
                    ProcedureId = payment.ProcedureId,
                    PaymentDate = payment.PaymentDate,
                    Amount = payment.Amount,
                    Method = payment.Method,
                    Notes = payment.Notes,
                };
            }
            return null;

        }

        public IEnumerable<Payment> GetAllPaymentsByServiceId(int serviceId)
        {
            var payments = _context.Payments.Where(p => p.ProcedureId == serviceId).ToList();
            if (!payments.Any()) { throw new InvalidOperationException("No payments found for this service"); }
            return payments;
        
        }

        public Payment GetPaymentByPaymentId(int paymentId)
        {
            var paymentToGet = _context.Payments.FirstOrDefault(p => p.Id == paymentId);
            if (paymentToGet == null) { throw new InvalidOperationException("Can not get payment."); }
            return paymentToGet;
        }

        public Payment UpdatePayment(Payment payment)
        {
            if (payment == null) { throw new ArgumentNullException("Payment must be populated."); }
            var paymentToUpdate = _context.Payments.FirstOrDefault(p => p.Id == payment.Id);
            if (paymentToUpdate == null) { throw new InvalidOperationException("Payment does not exist."); }

            paymentToUpdate.PatientId = payment.PatientId;
            paymentToUpdate.ProcedureId = payment.ProcedureId;
            paymentToUpdate.Method = payment.Method;
            paymentToUpdate.Notes = payment.Notes;
            paymentToUpdate.PaymentDate = payment.PaymentDate;

            _context.SaveChanges();

            return paymentToUpdate;    
        }

        public void DeletePaymentById(int paymentId)
        {
            var paymentToDelete = _context.Payments.FirstOrDefault(p => p.Id == paymentId);
            if (paymentToDelete == null) { throw new InvalidOperationException("Payment does not exist."); }

            _context.Payments.Remove(paymentToDelete);
            _context.SaveChanges();
        }
    }
}
