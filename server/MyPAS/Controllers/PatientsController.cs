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

            var patient =  await _patientService.GetPatientById(id);
            if (patient == null) { return NotFound(); }
            return Ok(patient);
        }

        // Create patient
        [HttpPost]
        public async Task<ActionResult> AddPatient([FromBody] CreatePatientDTO createPatientDTO)
        {
            var result = await _patientService.CreatePatient(createPatientDTO);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // Update patient
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(int id, UpdatePatientDTO updatePatientDTO)
        {
            var result = await _patientService.UpdatePatient(id, updatePatientDTO);
            if (result == null) return NotFound();
            return Ok(result);
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
