using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Models;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureController : Controller
    {
        // Dependency Injection
        private readonly MyPASContext _context;

        public ProcedureController(MyPASContext context)
        {
            _context = context;
        }

        // Begin Http Methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Procedure>>> GetProceduress()
        {
            return await _context.Procedures.Include(p => p.Patient).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<Procedure>> GetProcedureById(int id)
        {
            var procedure = await _context.Procedures.Include(s => s.Patient).FirstOrDefaultAsync(s => s.Id == id);
            if (procedure == null)
                return NotFound();

            return procedure;
        }


        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Procedure>>> GetServicesByPatient(int patientId)
        {
            var services = await _context.Procedures
                .Where(p => p.PatientId == patientId)
                .Include(p => p.Patient)
                .ToListAsync();

            return services;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Procedure procedure)
        {
            // check if the patient exists
            var patientExists = await _context.Patients.AnyAsync(p => p.Id == procedure.PatientId);
            if (!patientExists)
            {
                return BadRequest("Patient not found.");
            }

            _context.Procedures.Add(procedure);
            await _context.SaveChangesAsync();
            return Ok(procedure);
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
        public async Task<IActionResult> UpdateService(int id, Procedure procedure)
        {
            if (id != procedure.Id)
            {
                return BadRequest();
            }

            _context.Entry(procedure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Procedures.Any(e => e.Id == id))
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
            var service = await _context.Procedures.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Procedures.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
