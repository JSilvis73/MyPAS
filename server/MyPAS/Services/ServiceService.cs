using MyPAS.Models;
using MyPAS.Data;

namespace MyPAS.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly MyPASContext _context;

        public ProcedureService(MyPASContext context)
        {
            _context = context;
        }

        public Procedure CreateProcedureForPatientByPatientId(int patientId, string procedureName, decimal chargeAmt)
        {
            var procedure = new Procedure
            {
                PatientId = patientId,
                ProcedureName = procedureName,
                ProcedureDate = DateOnly.FromDateTime(DateTime.Now), // This will need changed to accept input.
                PatientChargedAmount = chargeAmt
            };

            _context.Procedures.Add(procedure);
            _context.SaveChanges();

            return procedure;
        }
        public IEnumerable<Procedure> GetAllproceduresForPatientById(int patientId)
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
