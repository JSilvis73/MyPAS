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

            if (string.IsNullOrWhiteSpace(createPatientDTO.Email) && string.IsNullOrWhiteSpace(createPatientDTO.Phone)) return null;

            var patient = new Patient 
            { 
                FirstName = createPatientDTO.FirstName, 
                LastName = createPatientDTO.LastName,
                Address = createPatientDTO.Address,
                City = createPatientDTO.City,
                State = createPatientDTO.State,
                Zip = createPatientDTO.Zip,
                Age = createPatientDTO.Age,
                Phone = createPatientDTO.Phone,
                Email = createPatientDTO.Email

            };

            _logger.LogInformation("Attempting to add patient: {LastName}, {FirstName} to database.", createPatientDTO.LastName, createPatientDTO.FirstName);
            try
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return new PatientDTO 
                { 
                    Id = patient.Id,
                    FirstName = patient.FirstName, 
                    LastName = patient.LastName,
                    Address = patient.Address,
                    City = patient.City,
                    State = patient.State,
                    Zip = patient.Zip,
                    Age = patient.Age,
                    Phone = patient.Phone,
                    Email = patient.Email
                    
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
            
            if (updatePatientDTO.FirstName!=null) patientToUpdate.FirstName = updatePatientDTO.FirstName;

            if (updatePatientDTO.LastName!=null) patientToUpdate.LastName = updatePatientDTO.LastName;
            if (updatePatientDTO.Address!=null) patientToUpdate.Address = updatePatientDTO.Address;
            if (updatePatientDTO.City!=null) patientToUpdate.City = updatePatientDTO.City;
            if (updatePatientDTO.State!=null) patientToUpdate.State = updatePatientDTO.State;
            if (updatePatientDTO.Zip!=null) patientToUpdate.Zip = updatePatientDTO.Zip;
            if (updatePatientDTO.Age.HasValue) patientToUpdate.Age = updatePatientDTO.Age.Value;
            if (updatePatientDTO.Phone!=null) patientToUpdate.Phone = updatePatientDTO.Phone;
            if (updatePatientDTO.Email!=null) patientToUpdate.Email = updatePatientDTO.Email;

        
            await _context.SaveChangesAsync();

            return new PatientDTO
            {
                Id = patientToUpdate.Id,
                FirstName = patientToUpdate.FirstName,
                LastName = patientToUpdate.LastName,
                Address = patientToUpdate.Address,
                City = patientToUpdate.City,
                State = patientToUpdate.State,
                Zip = patientToUpdate.Zip,
                Age = patientToUpdate.Age,
                Phone = patientToUpdate.Phone,
                Email = patientToUpdate.Email,
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
