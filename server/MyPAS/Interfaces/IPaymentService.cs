using MyPAS.Models;
using MyPAS.Models.DTO;

namespace MyPAS.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDTO?> CreatePaymentWithCreatePaymentDTO(CreatePaymentDTO createPaymentDTO);
        Task<List<PaymentDTO>> GetPaymentsByPatientId(int patientId);
        Task<List<PaymentDTO>> GetAllPaymentsByProcedureId(int procedureId);
        Task<PaymentDTO?> GetPaymentByPaymentId(int paymentId);
        Task<PaymentDTO?> UpdatePaymentByDTO(int paymentId, UpdatePaymentDTO updatePaymentDTO);
        Task<bool> DeletePaymentById(int paymentId);
    }
}
