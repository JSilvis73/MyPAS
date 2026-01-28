using MyPAS.Models;
using MyPAS.Data;

public class PatientService : IPatientService
    {
        private readonly MyPASContext _context;
    
        public PatientService(MyPASContext context)
        {
          _context = context;
        }

    public IEnumerable<Patient> GetAll() 
    {
        return _context.Patients.ToList();
    }

    public Patient GetById(int id) 
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
        var patient = GetById(id);
        _context.Patients.Remove(patient);
        _context.SaveChanges();

    }

}