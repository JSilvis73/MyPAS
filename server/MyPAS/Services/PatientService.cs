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
        private ILogger<PatientService> _logger;

        // Constructor
        public PatientService(MyPASContext context, ILogger<PatientService> logger)
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

        public async Task<PatientDTO> UpdatePatient(int id, UpdatePatientDTO updatePatientDTO)
        {
            var patientToUpdate = await _context.Patients.FindAsync(id);

            if (patientToUpdate == null) return null;

            patientToUpdate.FirstName = updatePatientDTO.FirstName;
            patientToUpdate.LastName = updatePatientDTO.LastName;
            patientToUpdate.Address = updatePatientDTO.Address;
            patientToUpdate.City = updatePatientDTO.City;
            patientToUpdate.State = updatePatientDTO.State;
            patientToUpdate.Zip = updatePatientDTO.Zip;
            patientToUpdate.Age = updatePatientDTO.Age;
            patientToUpdate.Phone = updatePatientDTO.Phone;
            patientToUpdate.Email = updatePatientDTO.Email;

        
            await _context.SaveChangesAsync();

            return new PatientDTO
            {
                Id = patientToUpdate.Id,
                FirstName = patientToUpdate.FirstName,
                LastName = patientToUpdate.LastName,
            };
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
