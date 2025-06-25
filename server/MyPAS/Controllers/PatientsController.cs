using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Models;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        // Dependenccy Injection - We inject our database into the controller so that it is able to perform the database accctions as needed.
        private readonly MyPASContext _context;

        // Constructor to assign context.
        public PatientsController(MyPASContext context)
        {
            _context = context;
        }

        // HTTP Methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }



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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            Console.WriteLine($"Received ID: {updatedPatient.Id}, Name: {updatedPatient.FirstName}");


            if (id != updatedPatient.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingPatient = await _context.Patients.FindAsync(id);

            if (existingPatient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            // Update fields manually (this protects against over-posting)
            existingPatient.FirstName = updatedPatient.FirstName;
            existingPatient.LastName = updatedPatient.LastName;
            existingPatient.Address = updatedPatient.Address;
            existingPatient.City = updatedPatient.City;
            existingPatient.State = updatedPatient.State;
            existingPatient.Zip = updatedPatient.Zip;
            existingPatient.Age = updatedPatient.Age;
            existingPatient.Phone = updatedPatient.Phone;
            existingPatient.Email = updatedPatient.Email;

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
