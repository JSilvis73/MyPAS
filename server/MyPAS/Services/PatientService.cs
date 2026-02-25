using MyPAS.Models;
using MyPAS.Data;
using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MyPAS.Services
{
    public class PatientService : IPatientService
    {

        // This service is used to:
        // Retrieve all patients, retrive a single patient by id, create a patient, update a patient and delete a patient.

        // Service for interacting with DB.
        private readonly MyPASContext _context;

        // Constructor
        public PatientService(MyPASContext context)
        {
            _context = context;
        }

        // CRUD
        public async Task<Patient> CreatePatient(string firstName, string lastName)
        {
            var patient = new Patient { FirstName = firstName, LastName = lastName };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient> GetPatientById(int id)
        {
            var patientToFind =  await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);

            return patientToFind
                ?? throw new InvalidOperationException("Patient Not Found");
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

        public async Task DeletePatient(int id)
        {
            var patient = await GetPatientById(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

   
    }
}
