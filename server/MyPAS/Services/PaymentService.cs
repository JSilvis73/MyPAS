using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Interfaces;
using MyPAS.Models;
using MyPAS.Models.DTO;
using System.Threading.Tasks;

namespace MyPAS.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly MyPASContext _context;
        private readonly ILogger _logger;


        public PaymentService(MyPASContext context, ILogger logger)
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

        public async Task<PaymentDTO?> GetPaymentByPaymentId(int paymentId)
        {
            var paymentToFind = await _context.Payments.FindAsync(paymentId);
            if (paymentToFind != null)
            {
                return new PaymentDTO
                {
                    Id = paymentToFind.Id,
                    PatientId = paymentToFind.PatientId,
                    ProcedureId = paymentToFind.ProcedureId,
                    PaymentDate = paymentToFind.PaymentDate,
                    Amount = paymentToFind.Amount,
                    Method = paymentToFind.Method,
                    Notes = paymentToFind.Notes
                };
            }
            return null;
        }

        public async Task<List<PaymentDTO>> GetAllPaymentsByProcedureId(int procedureId)
        {
            return await _context.Payments.Where(p => p.ProcedureId == procedureId)
                .Select(p => new PaymentDTO
                {
                    Id = p.Id,
                    PatientId = p.PatientId,
                    ProcedureId = p.ProcedureId,
                    PaymentDate = p.PaymentDate,
                    Amount = p.Amount,
                    Method = p.Method,
                    Notes = p.Notes

                }).ToListAsync();
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
           
        }
    }
}
