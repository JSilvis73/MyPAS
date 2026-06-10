using MyPAS.Models;
using MyPAS.Models.DTO;

namespace MyPAS.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDTO?> CreatePaymentWithCreatePaymentDTO(CreatePaymentDTO createPaymentDTO);
        IEnumerable<Payment> GetAllPaymentsByServiceId(int serviceId);
        Task<PaymentDTO> GetPaymentByPaymentId(int paymentId);
        Payment UpdatePayment(Payment payment);
        void DeletePaymentById(int paymentId);
    }
}
