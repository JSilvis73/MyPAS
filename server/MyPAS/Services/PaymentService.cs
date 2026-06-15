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
        private readonly ILogger<PaymentService> _logger;


        public PaymentService(MyPASContext context, ILogger<PaymentService> logger)
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
                    ProcedureId = createPaymentDTO.ProcedureId,
                    PaymentDate = createPaymentDTO.PaymentDate,
                    Amount = createPaymentDTO.Amount,
                    Method = createPaymentDTO.Method,
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

                }).ToListAsync();
        }



        public async Task<PaymentDTO?> UpdatePaymentByDTO(int paymentId,UpdatePaymentDTO updatePaymentDTO)
        {
            var paymentToUpdate = await _context.Payments.FindAsync(paymentId);
            if (paymentToUpdate == null) return null;
            
            if (updatePaymentDTO.Amount.HasValue)
                paymentToUpdate.Amount = updatePaymentDTO.Amount.Value;

            if (updatePaymentDTO.Method!=null)
            paymentToUpdate.Method = updatePaymentDTO.Method;

            if (updatePaymentDTO.PaymentDate.HasValue)
                paymentToUpdate.PaymentDate = updatePaymentDTO.PaymentDate.Value;
      

                await _context.SaveChangesAsync();

                return new PaymentDTO
                {
                    Id = paymentToUpdate.Id,
                    PatientId = paymentToUpdate.PatientId,
                    ProcedureId = paymentToUpdate.ProcedureId,
                    PaymentDate = paymentToUpdate.PaymentDate,
                    Amount = paymentToUpdate.Amount,
                    Method = paymentToUpdate.Method,
               }; 
        }

        public async Task<bool> DeletePaymentById(int paymentId)
        {
            var paymentToDelete = await _context.Payments.FindAsync(paymentId);
            if (paymentToDelete != null)
            {
                _context.Payments.Remove(paymentToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
