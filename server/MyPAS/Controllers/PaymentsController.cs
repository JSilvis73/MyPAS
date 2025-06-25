using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyPAS.Data;
using MyPAS.Models;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly MyPASContext _context;
        public PaymentsController(MyPASContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {

            return await _context.Payments.Include(p => p.Service).ToListAsync();
        }

        [HttpGet("{paymentId}")]
        public async Task<ActionResult<Payment>> GetPayment(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Service)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpGet("patients/{patientId}/payments")]
        public async Task<ActionResult<List<Payment>>> GetPaymentsByPatientId(int patientId)
        {
            try
            {
                var payments = await _context.Payments
                    .Where(p => p.PatientId == patientId)
                    .Include(p => p.Patient)
                    .ToListAsync();

                if (payments == null || !payments.Any())
                {
                    return NotFound();
                }

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("services/{serviceId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByService(int serviceId)
        {
            
            var payments = await _context.Payments
                
                .Where(p => p.ServiceId == serviceId)
                .Include(p => p.Service).ToListAsync();

            if (payments == null || payments.Count == 0)
            {
                return NotFound($"No payments found for this service Id.");
            }

            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(payment);
        }

    }
}
