using MyPAS.Models;
using MyPAS.Data;
using MyPAS.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MyPAS.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly MyPASContext _context;
        private readonly ILogger<ProcedureService> _logger;

        public ProcedureService(MyPASContext context, ILogger<ProcedureService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProcedureDTO?> CreateProcedureForPatientByCreateProcedureDTO(CreateProcedureDTO createProedureDTO)
        {
            _logger.LogInformation("Attempting to create procedure: {ProcedureName}", createProedureDTO.ProcedureName);

            var patient = await _context.Patients.FindAsync(createProedureDTO.PatientId);
            if (patient == null) return null;

            Procedure procedureToCreate = new Procedure
            {
                PatientId = createProedureDTO.PatientId,
                ProcedureName = createProedureDTO.ProcedureName,
                ProcedureDate = createProedureDTO.ProcedureDate,
                PatientChargedAmount =(decimal) createProedureDTO.PatientChargedAmount,
                CptAmount = createProedureDTO.CptAmount,
                CptCode = createProedureDTO.CptCode,
            };

            await _context.Procedures.AddAsync(procedureToCreate);
            await _context.SaveChangesAsync();

            return new ProcedureDTO
            {
                Id = procedureToCreate.Id,
                PatientId = (int) procedureToCreate.PatientId,
                ProcedureName = procedureToCreate.ProcedureName,
                PatientChargedAmount = (decimal)procedureToCreate.PatientChargedAmount,
                CptAmount= (decimal) procedureToCreate.CptAmount,
                CptCode= procedureToCreate.CptCode,
                ProcedureDate= procedureToCreate.ProcedureDate,

            };
        }

        public async Task<List<ProcedureDTO>> GetAllProceduresForPatientByPatientId(int patientId)
        {
            return await _context.Procedures.Where(
                p => p.PatientId == patientId)
                .Select(p => new ProcedureDTO
                {
                    Id = p.Id,
                    PatientId= (int) p.PatientId,
                    ProcedureName= p.ProcedureName,
                    ProcedureDate = p.ProcedureDate,
                    PatientChargedAmount= (decimal) p.PatientChargedAmount,
                    CptAmount = (decimal) p.CptAmount,
                    CptCode= p.CptCode,

                })
                .ToListAsync();
        }

        public async Task<ProcedureDTO?> GetProcedureByProcedureId(int id)
        {
            return await _context.Procedures
             .Where(p => p.Id == id)
             .Select(p => new ProcedureDTO
             {
                 Id = p.Id,
                 ProcedureName = p.ProcedureName,
                 ProcedureDate = p.ProcedureDate,
                 PatientId = (int) p.PatientId,
                 PatientChargedAmount = (decimal) p.PatientChargedAmount,
                 CptAmount = (decimal) p.CptAmount,
                 CptCode = p.CptCode
             })
             .SingleOrDefaultAsync();
        }

        public async Task<ProcedureDTO?> UpdateProcedureByUpdateProcedureDTO(int procedureId, UpdateProcedureDTO updateProcedureDTO)
        {
            // Locate procedure
            var procedureToUpdate = await _context.Procedures.FindAsync(procedureId);
            if (procedureToUpdate != null)
            { 
                // Update fields if procedure is found.
                if (updateProcedureDTO != null)
                procedureToUpdate.ProcedureName = updateProcedureDTO.ProcedureName;

                if (updateProcedureDTO.ProcedureDate.HasValue)
                procedureToUpdate.ProcedureDate = updateProcedureDTO.ProcedureDate.Value;

                if (procedureToUpdate.PatientChargedAmount != null)
                procedureToUpdate.PatientChargedAmount = updateProcedureDTO.PatientChargedAmount;

                if (procedureToUpdate.CptCode != null)
                procedureToUpdate.CptCode = updateProcedureDTO.CptCode;

                if (procedureToUpdate.CptAmount != null)
                procedureToUpdate.CptAmount = updateProcedureDTO.CptAmount;
          
                await _context.SaveChangesAsync();

                return new ProcedureDTO
                {
                    PatientId = procedureToUpdate.PatientId,
                    Id = procedureToUpdate.Id,
                    ProcedureName = procedureToUpdate.ProcedureName,
                    ProcedureDate = procedureToUpdate.ProcedureDate,
                    PatientChargedAmount = procedureToUpdate.PatientChargedAmount,
                    CptCode = procedureToUpdate.CptCode,
                    CptAmount = procedureToUpdate.CptAmount,
                };
            }

            return null;
        }

        public async Task<bool> DeleteProcedureByProcedureId(int id)
        {
            var procedure = await _context.Procedures.FindAsync(id);

            if (procedure is null)
                return false;

            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
