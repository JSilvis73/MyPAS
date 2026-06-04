using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using MyPAS.Data;
using MyPAS.Models;
using Serilog;
using MyPAS.Services;
using MyPAS.Models.DTO;

namespace MyPAS.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private IPatientService _patientService;
        public PatientsController(IPatientService patientService) { _patientService = patientService; }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {  
            var result = await _patientService.GetAllPatients();

            if (result == null) return NotFound();

            return Ok(result); 
        }

        // Get specific patient by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
         
            
            try
            {
                var patient =  await _patientService.GetPatientById(id);
                if (patient == null) { return NotFound(); }

           
                return Ok(patient);
   
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, " An internal server error occurred.");
            }
        }

        // Create patient
        //[HttpPost]
        //public async Task<ActionResult> AddPatient([FromBody] Patient patient)
        //{
            

        //    try
        //    {
        //        if (patient == null)
        //        {
                  
        //            return BadRequest("Patient could not be added.");
        //        }
        //        await _patientService.CreatePatient(patient.FirstName, patient.LastName);

             
        //        return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
        //    }
        //    catch (Exception ex)
        //    {
               
        //        return StatusCode(500, "An internal server error occurred.");
        //    }
        //}

        // Update patient
        //[HttpPut("{id}")]
        //public async Task<ActionResult> UpdatePatient(int id, [FromBody] Patient patientToUpdate)
        //{
           
        //        patientToUpdate.LastName, patientToUpdate.FirstName);

        //    try
        //    {
        //        if (patientToUpdate == null) return BadRequest();
        //        await _patientService.UpdatePatient(patientToUpdate);
                
               
        //        return Ok(patientToUpdate);
        //    }
        //    //catch (DbUpdateConcurrencyException ex)
        //    //{
        //    //    _logger.LogError(ex, $"Concurrency error while updating patient ID: {id}");
        //    //    if (!_context.Patients.Any(p => p.Id == id))
        //    //    {
        //    //        return NotFound();
        //    //    }

        //    //    throw; // optional: rethrow if you're in dev and want to crash upward
        //    //}
        //    catch (Exception ex)
        //    {
                
        //        return StatusCode(500, "An internal server error occurred.");
        //    }
        //}


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            

            try
            {
                await _patientService.DeletePatient(id);

               
                return NoContent();
            }
            catch (Exception ex) 
            {
                
                return StatusCode(500, "An internal server error occurred.");
            }

        }

    }
}
