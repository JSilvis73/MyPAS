using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Models;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : Controller
    {
        // Dependency Injection
        private readonly MyPASContext _context;

        public ServicesController(MyPASContext context)
        {
            _context = context;
        }

        // Begin Http Methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.Include(s => s.Patient).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<Service>> GetServiceById(int id)
        {
            var service = await _context.Services.Include(s => s.Patient).FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
                return NotFound();

            return service;
        }


        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesByPatient(int patientId)
        {
            var services = await _context.Services
                .Where(s => s.PatientId == patientId)
                .Include(s => s.Patient)
                .ToListAsync();

            return services;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            // check if the patient exists
            var patientExists = await _context.Patients.AnyAsync(p => p.Id == service.PatientId);
            if (!patientExists)
            {
                return BadRequest("Patient not found.");
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return Ok(service);
        }
        //[HttpPost]
        //public async Task<ActionResult<Service>> CreateService([FromBody] Service service)
        //{
        //    if (service == null) { return BadRequest(); }   // Sends 400 Bad Request.

        //    _context.Services.Add(service);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
        //}



        // PUT: api/service/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Services.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/service/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
