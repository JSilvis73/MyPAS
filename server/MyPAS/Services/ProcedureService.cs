using MyPAS.Models;
using MyPAS.Data;
using MyPAS.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyPAS.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly MyPASContext _context;
        private readonly ILogger _logger;

        public ProcedureService(MyPASContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProcedureDTO?> CreateProcedureForPatientByCreateProcedureDTO(CreateProcedureDTO createProedureDTO)
        {
            _logger.LogInformation("Attempting to create procedure: {ProcedureName}", createProedureDTO.ProcedureName);

            var patient = await _context.Patients.FindAsync(createProedureDTO.PatientId);
            if (patient == null) return null;

            var procedureToCreate = new Procedure
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
                PatientId = procedureToCreate.PatientId,
                ProcedureName = procedureToCreate.ProcedureName,
                PatientChargedAmount = procedureToCreate.PatientChargedAmount,
                CptAmount= procedureToCreate.CptAmount,
                CptCode= procedureToCreate.CptCode,
                ProcedureDate= procedureToCreate.ProcedureDate,

            };
        }

        public IEnumerable<Procedure> GetAllProceduresForPatientById(int patientId)
        {
            var services = _context.Procedures.Where(p => p.PatientId == patientId).ToList();
            if (!services.Any()) { throw new InvalidOperationException("No procedures for this patient."); }
            return services;
        }

        public Procedure GetProcedureById(int id)
        {
            return _context.Procedures.FirstOrDefault(s => s.Id == id)
                ?? throw new Exception("Procedure does not exist.");
        }

        public Procedure UpdateProcedureById(int id, Procedure procedure)
        {
            // Find service
            var procedureToUpdate = _context.Procedures.FirstOrDefault(s => s.Id == id)
            ?? throw new InvalidOperationException("Procedure does not exist with that id.");

            // Update service
            procedureToUpdate.PatientId = procedure.PatientId;
            procedureToUpdate.ProcedureName = procedure.ProcedureName;
            procedureToUpdate.ProcedureDate = procedure.ProcedureDate;
            procedureToUpdate.PatientChargedAmount = procedure.PatientChargedAmount;
            procedureToUpdate.CptCode = procedure.CptCode;
            procedureToUpdate.CptAmount = procedure.CptAmount;

            // Save changes to DB
            _context.SaveChanges();

            return procedureToUpdate;
        }

        public void DeleteProcedureById(int id)
        {
            var procedureToRemove = _context.Procedures.FirstOrDefault(p => p.Id == id);
            if (procedureToRemove == null)
            {
                throw new Exception("Procedure not found.");
            }

            _context.Procedures.Remove(procedureToRemove);
            _context.SaveChanges();

        }
    }
}
