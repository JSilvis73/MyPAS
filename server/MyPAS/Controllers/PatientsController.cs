using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Models;
using Serilog;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        // Dependency Injection - We inject our database into the controller so that it is able to perform the database actions as needed.
        private readonly MyPASContext _context;
        private readonly ILogger<PatientsController> _logger;


        // Constructor to assign context.
        public PatientsController(ILogger<PatientsController> logger, MyPASContext context)
        {
            _logger = logger;
            _context = context;
        }

     

        // HTTP Methods

        // Get list of patients.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            _logger.LogInformation("Attempting to fetching all patients...");

            try
            {
                var patients = await _context.Patients.ToListAsync();
                if (patients == null || patients.Count == 0)
                {
                    _logger.LogWarning("Patients not found in the database.");
                    return NotFound("No patients found.");
                }

                _logger.LogInformation("Successfully fetched {Count} patients.", patients.Count);
                return Ok(patients);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while fetching patients.");
                return StatusCode(500, "An internal server error occurred.");
            }   
        }

        // Get specific patient by ID.
        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatient(int id)
        {
            _logger.LogInformation($"Fetching patient with ID: {id}");
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        // Create patient
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest(); // Sends a 400 Bad Request.
            }

            _context.Patients.Add(patient);     // Prepares the new patient to be saved.
            await _context.SaveChangesAsync();   // Atually saves the changes to db.

            return CreatedAtAction(nameof(GetPatients), new { id = patient.Id}, patient);
        }

        // Update patient
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patientToUpdate)
        {
            // Output updated patient
            Console.WriteLine($"Received ID: {patientToUpdate.Id}, Name: {patientToUpdate.FirstName}");

            if (id != patientToUpdate.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingPatient = await _context.Patients.FindAsync(id);

            if (existingPatient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            // Update fields manually (this protects against over-posting)
            existingPatient.FirstName = patientToUpdate.FirstName;
            existingPatient.LastName = patientToUpdate.LastName;
            existingPatient.Address = patientToUpdate.Address;
            existingPatient.City = patientToUpdate.City;
            existingPatient.State = patientToUpdate.State;
            existingPatient.Zip = patientToUpdate.Zip;
            existingPatient.Age = patientToUpdate.Age;
            existingPatient.Phone = patientToUpdate.Phone;
            existingPatient.Email = patientToUpdate.Email;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingPatient); // Return updated patient
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Patients.Any(p => p.Id == id))
                {
                    return NotFound();
                }

                throw; // Let the pipeline handle it (for dev purposes)
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
