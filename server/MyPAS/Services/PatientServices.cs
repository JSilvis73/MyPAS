using MyPAS.Models;
using MyPAS.Data;
using System.Diagnostics.Eventing.Reader;

public class PatientService : IPatientService
    {

    // This service is used to:
    // Retrieve all patients, retrive a single patient by id, create a patient, delete a patient.

    // Services for interacting with DB.
        private readonly MyPASContext _context;
    
        public PatientService(MyPASContext context)
        {
          _context = context;
        }


    // Service Methods
    public IEnumerable<Patient> GetAllPatients() 
    {
        return _context.Patients.ToList();
    }

    public Patient GetPatientById(int id) 
    {
        var patientToFind = _context.Patients.FirstOrDefault(p => p.Id == id);

        return patientToFind
            ?? throw new InvalidOperationException("Patient Not Found");
        
    }

    public Patient CreatePatient(string firstName, string lastName)
    {
        var patient = new Patient { FirstName = firstName, LastName = lastName };

        _context.Patients.Add(patient);
        _context.SaveChanges();
        return patient;

    }

    public void DeletePatient(int id)
    {
        var patient = GetPatientById(id);
        _context.Patients.Remove(patient);
        _context.SaveChanges();
    }

    public Patient UpdatePatient(Patient patient)
    {
        //if (patient == null) { throw new ArgumentException("Patient not found"); }
       
        var patientToUpdate = _context.Patients.FirstOrDefault(p => p.Id == patient.Id);
        if (patientToUpdate == null) { throw new InvalidOperationException("Patient does not exist in memory."); }

        patientToUpdate.FirstName = patient.FirstName;
        patientToUpdate.LastName = patient.LastName;
        patientToUpdate.Age = patient.Age;
        patientToUpdate.Email = patient.Email;
        patientToUpdate.Phone = patient.Phone;
        patientToUpdate.Address = patient.Address;
        patientToUpdate.City = patient.City;
        patientToUpdate.State = patient.State;
        patientToUpdate.Zip = patient.Zip;
            
        
        _context.SaveChanges();
        return patientToUpdate;

    }


}