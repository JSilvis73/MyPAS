using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            _logger.LogInformation("Attempting to fetch patient with ID: {id}", id);

            try
            {
                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID: {id}, not found in the database.", id);
                    return NotFound($"No patient with ID: {id} found.");
                }
                _logger.LogInformation("Patient with ID:{id} found.", id);
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching patient with ID: {id}", id);
                return StatusCode(500, " An internal server error occurred.");
            }
        }

        // Create patient
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            _logger.LogInformation("Attempting to add patient to database...");

            try
            {
                if (patient == null)
                {
                    _logger.LogWarning("Patient could not be added.");
                    return BadRequest("Patient could not be added.");
                }
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Patient with ID: {id} has been added.", patient.Id);
                return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding patient.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Update patient
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patientToUpdate)
        {
            _logger.LogInformation("Attempting to update patient: {LastName}, {FirstName}.",
                patientToUpdate.LastName, patientToUpdate.FirstName);

            if (id != patientToUpdate.Id)
            {
                _logger.LogWarning("Patient ID mismatch. URL ID: {id}, Body ID: {bodyId}", id, patientToUpdate.Id);
                return BadRequest("ID mismatch");
            }

            var existingPatient = await _context.Patients.FindAsync(id);

            if (existingPatient == null)
            {
                _logger.LogWarning("Patient with ID: {patientID} not found.", id);
                return NotFound($"Patient with ID {id} not found.");
            }

            // Update fields
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
                _logger.LogInformation("Patient with ID: {id} successfully updated.", id);
                return Ok(existingPatient);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while updating patient ID: {id}", id);
                if (!_context.Patients.Any(p => p.Id == id))
                {
                    return NotFound();
                }

                throw; // optional: rethrow if you're in dev and want to crash upward
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating patient ID: {id}", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            _logger.LogInformation("Attempting to delete patient with ID: {id}.", id);

            try
            {
                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID: {id} not found in the database.", id);
                    return NotFound($"Patient with ID:{id} was not found.");
                }

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Patient with ID:{id} deleted successfully.",id);
                return NoContent();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting patient ID: {id}", id);
                return StatusCode(500, "An internal server error occurred.");
            }

        }

    }
}
