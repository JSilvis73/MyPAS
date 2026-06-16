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

            return Ok(result); 
        }

        // Get specific patient by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatientById(int id)
        { 

            var patient =  await _patientService.GetPatientById(id);
            if (patient != null) { return Ok(patient); }
            return NotFound();
           
        }

        // Create patient
        [HttpPost]
        public async Task<ActionResult<PatientDTO>> AddPatient([FromBody] CreatePatientDTO createPatientDTO)
        {
            var result = await _patientService.CreatePatient(createPatientDTO);
            if (result != null) return CreatedAtAction(nameof(GetPatientById), new { id = result.Id}, result);
            return BadRequest();
        }

        // Update patient
        [HttpPatch("{id}")]
        public async Task<ActionResult<PatientDTO>> UpdatePatient(int id, [FromBody] UpdatePatientDTO updatePatientDTO)
        {
            var result = await _patientService.UpdatePatient(id, updatePatientDTO);
            if (result != null) return Ok(result);
            return NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeletePatient(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
