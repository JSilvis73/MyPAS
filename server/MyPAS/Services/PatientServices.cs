using MyPAS.Models;

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

    public Patient CreatePatient(Patient patient)
    {

    }

    public void DeletePatient(int id)
    {

    }

}