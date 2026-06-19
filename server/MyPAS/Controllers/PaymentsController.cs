using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyPAS.Data;
using MyPAS.Interfaces;
using MyPAS.Models;
using MyPAS.Models.DTO;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly MyPASContext _context;
        private readonly IPaymentService _paymentService;
        public PaymentsController(MyPASContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        [HttpGet("{paymentId}")]
        public async Task<ActionResult<PaymentDTO>> GetPaymentByPaymentId(int paymentId)
        {
            var result = await _paymentService.GetPaymentByPaymentId(paymentId);
            return Ok(result);
        }

        [HttpGet("patients/{patientId}/payments")]
        public async Task<ActionResult<List<PaymentDTO>>> GetPaymentsByPatientId(int patientId)
        {
            var results = await _paymentService.GetPaymentsByPatientId(patientId);
            return Ok(results);
        }

        [HttpGet("procedure/{procedureId}/payments")]
        public async Task<ActionResult<List<PaymentDTO>>> GetPaymentsByProcedureId(int procedureId)
        {
            var result = await _paymentService.GetAllPaymentsByProcedureId(procedureId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> AddPayment([FromBody] CreatePaymentDTO createPaymentDTO)
        {
            var result = await _paymentService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);
            if (result != null) { return CreatedAtAction(nameof(GetPaymentByPaymentId), new { paymentId = result.Id }, result); }
            return BadRequest();
        }

        [HttpPatch("/update/{paymentId}")]
        public async Task<ActionResult<PaymentDTO?>> UpdatePaymentByPaymentId(int paymentId, [FromBody] UpdatePaymentDTO dto)
        {
            var result = await _paymentService.UpdatePaymentByDTO(paymentId, dto);
            if (result != null) { return Ok(result); }
            return BadRequest();
        }

        [HttpDelete("{paymentId}")]
        public async Task<ActionResult<bool>> DeletePaymentByPAymentId(int paymentId)
        {
            var result = await _paymentService.DeletePaymentById(paymentId);
            if (result == null) { return BadRequest(); }
            return NoContent();  
        }


    }
}
