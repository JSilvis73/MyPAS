using MyPAS.Models;
using MyPAS.Models.DTO;

namespace MyPAS.Interfaces
{
    public interface IPaymentServices
    {
        Task<PaymentDTO?> CreatePaymentWithCreatePaymentDTO(CreatePaymentDTO createPaymentDTO);
        IEnumerable<Payment> GetAllPaymentsByServiceId(int serviceId);
        Payment GetPaymentByPaymentId(int paymentId);
        Payment UpdatePayment(Payment payment);
        void DeletePaymentById(int paymentId);
    }
}
