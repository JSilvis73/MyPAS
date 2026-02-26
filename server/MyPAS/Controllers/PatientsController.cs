using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using MyPAS.Data;
using MyPAS.Models;
using Serilog;
using MyPAS.Services;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class PatientsController : ControllerBase
    {
        // Dependency Injection - 
        private readonly ILogger<PatientsController> _logger;
        private IPatientService _patientService;
     


        // Constructor to assign context.
        public PatientsController(ILogger<PatientsController> logger, IPatientService patientService)
        {
            _logger = logger;
            _patientService = patientService;
           
        }


        // HTTP Methods

        // Get list of patients.
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            _logger.LogInformation("Attempting to fetching all patients...");

            try
            {
                return Ok(_patientService.GetAllPatients());
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while fetching patients.");
                return StatusCode(500, "An internal server error occurred.");
            }   
        }

        // Get specific patient by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            _logger.LogInformation($"Attempting to fetch patient with ID: {id}");
            
            try
            {
                var patient =  await _patientService.GetPatientById(id);
                if (patient == null) { return NotFound(); }

                _logger.LogInformation($"Patient with ID:{id} found.");
                return Ok(_patientService.GetPatientById(id));
   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while fetching patient with ID: {id}");
                return StatusCode(500, " An internal server error occurred.");
            }
        }

        // Create patient
        [HttpPost]
        public async Task<ActionResult> AddPatient([FromBody] Patient patient)
        {
            _logger.LogInformation("Attempting to add patient to database...");

            try
            {
                if (patient == null)
                {
                    _logger.LogWarning("Patient could not be added.");
                    return BadRequest("Patient could not be added.");
                }
                await _patientService.CreatePatient(patient.FirstName, patient.LastName);

                _logger.LogInformation("Patient with ID: {id} has been added.", patient.Id);
                return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding patient.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Update patient
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(int id, [FromBody] Patient patientToUpdate)
        {
            _logger.LogInformation("Attempting to update patient: {LastName}, {FirstName}.",
                patientToUpdate.LastName, patientToUpdate.FirstName);

            try
            {
                if (patientToUpdate == null) return BadRequest();
                await _patientService.UpdatePatient(patientToUpdate);
                
                _logger.LogInformation($"Patient with ID: {id} successfully updated.");
                return Ok(patientToUpdate);
            }
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    _logger.LogError(ex, $"Concurrency error while updating patient ID: {id}");
            //    if (!_context.Patients.Any(p => p.Id == id))
            //    {
            //        return NotFound();
            //    }

            //    throw; // optional: rethrow if you're in dev and want to crash upward
            //}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating patient ID: {id}", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            _logger.LogInformation($"Attempting to delete patient with ID: {id}.");

            try
            {
                await _patientService.DeletePatient(id);

                _logger.LogInformation($"Patient with ID:{id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Unexpected error occurred while deleting patient ID: {id}");
                return StatusCode(500, "An internal server error occurred.");
            }

        }

    }
}
