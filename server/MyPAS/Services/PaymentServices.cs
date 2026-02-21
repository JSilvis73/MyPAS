using MyPAS.Data;
using MyPAS.Interfaces;
using MyPAS.Models;

namespace MyPAS.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly MyPASContext _context;

        public PaymentServices(MyPASContext context)
        {
            _context = context;
        }


        public Payment CreatePayment(int patientId, string payMethod, decimal payAmount)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == patientId);
            if (patient == null) { throw new InvalidOperationException($"Patient with id:{patientId} does not exist."); }

            var payment = new Payment
            { 
                PatientId = patientId,
                Method = payMethod,
                Amount = payAmount,
                PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                Patient = patient,
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment;
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
