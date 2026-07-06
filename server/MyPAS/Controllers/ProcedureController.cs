using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Models;
using MyPAS.Models.DTO;

namespace MyPAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureController : Controller
    {
        // Dependency Injection
        private readonly MyPASContext _context;
        private readonly IProcedureService _procedureService;

        public ProcedureController(MyPASContext context, IProcedureService procedureService)
        {
            _context = context;
            _procedureService = procedureService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProcedureDTO?>> GetProcedureById(int id)
        {
            var procedure = await _procedureService.GetProcedureByProcedureId(id);
            if (procedure == null)
            {
                return NotFound();
            };

            return Ok(procedure);
        }


        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<ProcedureDTO>>> GetAllProceduresForPatientByPatientId(int patientId)
        {
            return Ok(await _procedureService.GetAllProceduresForPatientByPatientId(patientId));
            
        }


        [HttpPost]
        public async Task<ActionResult<ProcedureDTO?>> CreateProcedureForPatientByCreateProcedureDTO([FromBody] CreateProcedureDTO createProcedureDTO)
        {
            var procedure = await _procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
            if (procedure != null) { return CreatedAtAction(nameof(CreateProcedureForPatientByCreateProcedureDTO), new { id = procedure.Id }, procedure); }
            return BadRequest();
        }



        // PUT: api/service/5
        [HttpPatch("{id}")]
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
