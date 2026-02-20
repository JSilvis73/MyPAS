using MyPAS.Models;

namespace MyPAS.Interfaces
{
    public interface IPaymentServices
    {
        Payment CreatePayment(int patientId, string payMethod, decimal payAmount);
        IEnumerable<Payment> GetAllPaymentsByServiceId(int serviceId);
        Payment GetPaymentByPaymentId(int paymentId);
        Payment UpdatePayment(Payment payment);
        void DeletePaymentById(int paymentId);
    }
}
