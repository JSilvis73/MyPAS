using MyPAS.Models;
using MyPAS.Data;
using MyPAS.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyPAS.Models.Enums;

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

            var patient =  _context.Patients.FirstOrDefault(p => p.Id == createProedureDTO.PatientId);
            if (patient == null) return null;

            if (string.IsNullOrWhiteSpace(createProedureDTO.ProcedureName) || createProedureDTO.PatientChargedAmount <= 0 || createProedureDTO.ProcedureDate == DateOnly.MinValue) return null;

            Procedure procedureToCreate = new Procedure
            {
                PatientId = createProedureDTO.PatientId,
                Patient = patient,
                ProcedureName = createProedureDTO.ProcedureName,
                ProcedureDate = createProedureDTO.ProcedureDate,
                PatientChargedAmount = createProedureDTO.PatientChargedAmount,
                CptAmount = createProedureDTO.CptAmount,
                CptCode = createProedureDTO.CptCode,
            };

            await _context.Procedures.AddAsync(procedureToCreate);
            await _context.SaveChangesAsync();

            return new ProcedureDTO
            {
                Id = procedureToCreate.Id,
                PatientId = procedureToCreate.PatientId,
                ProcedureName = procedureToCreate.ProcedureName,
                PatientChargedAmount = procedureToCreate.PatientChargedAmount,
                CptAmount= procedureToCreate.CptAmount,
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
                    PatientId= p.PatientId,
                    ProcedureName= p.ProcedureName,
                    ProcedureDate = p.ProcedureDate,
                    PatientChargedAmount= p.PatientChargedAmount,
                    CptAmount = p.CptAmount,
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
                 PatientId = p.PatientId,
                 PatientChargedAmount = p.PatientChargedAmount,
                 CptAmount = p.CptAmount,
                 CptCode = p.CptCode
             })
             .SingleOrDefaultAsync();
        }

        public async Task<ProcedureDTO?> UpdateProcedureByUpdateProcedureDTO(int procedureId, UpdateProcedureDTO updateProcedureDTO)
        {
            // Locate procedure
            var procedureToUpdate = await _context.Procedures.FindAsync(procedureId);
            if (procedureToUpdate == null) return null;
            
                // Update fields if procedure is found.
                if (!string.IsNullOrWhiteSpace(updateProcedureDTO.ProcedureName))
                procedureToUpdate.ProcedureName = updateProcedureDTO.ProcedureName;

                if (updateProcedureDTO.ProcedureDate.HasValue)
                procedureToUpdate.ProcedureDate = updateProcedureDTO.ProcedureDate.Value;

                if (updateProcedureDTO.PatientChargedAmount.HasValue)
                procedureToUpdate.PatientChargedAmount = updateProcedureDTO.PatientChargedAmount.Value;

                if (!string.IsNullOrWhiteSpace(updateProcedureDTO.CptCode))
                procedureToUpdate.CptCode = updateProcedureDTO.CptCode;

                if (updateProcedureDTO.CptAmount.HasValue)
                procedureToUpdate.CptAmount = updateProcedureDTO.CptAmount.Value;
          
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


        public async Task<DeleteProcedureResult> DeleteProcedureByProcedureId(int id)
        {
            var procedure = await _context.Procedures
                .Include(p => p.Payments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (procedure is null)
                return DeleteProcedureResult.NotFound;

            if (procedure.Payments.Any())
                return DeleteProcedureResult.HasPayments;

            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync();

            return DeleteProcedureResult.Deleted;
        }
    }
}
