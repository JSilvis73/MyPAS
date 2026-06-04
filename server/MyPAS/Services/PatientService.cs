using MyPAS.Models;
using MyPAS.Data;
using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using MyPAS.Models.DTO;

namespace MyPAS.Services
{
    public class PatientService : IPatientService
    {
        private readonly MyPASContext _context;
        private ILogger _logger;

        // Constructor
        public PatientService(MyPASContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // CRUD
        public async Task<PatientDTO?> CreatePatient(CreatePatientDTO createPatientDTO)
        {
            _logger.LogInformation("Attempting to create patient: {LastName}, {FirstName}.",createPatientDTO.LastName, createPatientDTO.FirstName);

            var patient = new Patient { FirstName = createPatientDTO.FirstName, LastName = createPatientDTO.LastName };

            _logger.LogInformation("Attempting to add patient: {LastName}, {FirstName} to database.", createPatientDTO.LastName, createPatientDTO.FirstName);
            try
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return new PatientDTO 
                { 
                    Id = patient.Id,
                    FirstName = patient.FirstName, 
                    LastName = patient.LastName 
                };
               
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to create patient {LastName}, {FirstName}", createPatientDTO.LastName, createPatientDTO.FirstName);
                throw;
            }
        }

        public async Task<List<PatientDTO>> GetAllPatients()
        {
            return await _context.Patients.Select(p => new PatientDTO
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName
            }
            ).ToListAsync();
        }

        public async Task<PatientDTO> GetPatientById(int id)
        {
            var patientToFind =  await _context.Patients.FindAsync(id);
            if (patientToFind == null) return null;

            return new PatientDTO
            {
                Id = patientToFind.Id,
                FirstName = patientToFind.FirstName,
                LastName = patientToFind.LastName
            };
        
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            var patientToUpdate = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patient.Id);
            if (patientToUpdate == null) { throw new InvalidOperationException("Patient does not exist in memory."); }

            _context.Entry(patientToUpdate).CurrentValues.SetValues(patient);
            //patientToUpdate.FirstName = patient.FirstName;
            //patientToUpdate.LastName = patient.LastName;
            //patientToUpdate.Age = patient.Age;
            //patientToUpdate.Email = patient.Email;
            //patientToUpdate.Phone = patient.Phone;
            //patientToUpdate.Address = patient.Address;
            //patientToUpdate.City = patient.City;
            //patientToUpdate.State = patient.State;
            //patientToUpdate.Zip = patient.Zip;

            await _context.SaveChangesAsync();
            return patientToUpdate;
        }

        public async Task<bool> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;
            
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            return true;
           
        }

   
    }
}
