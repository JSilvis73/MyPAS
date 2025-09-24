using MyPAS.Models;

public class PatientServices: IPatientServices
{
    MyPASContext _context;

    public PatientServices(MyPASContext context)
    {
        _context = context;
    }


    // Find All Patients

    // Find Patient by Id
    public Patient GetPatientById(int id)
    { 
        var patient = _context.Patients.Find(id);
        if (patient == null) { return null; }
        return patient;
    }
    // Create Patient
    public Patient CreatePatient(Patient patient)
    {
        _context.Patients.Add(patient);
        return patient;
    }

    // Update Patient
    public Patient UpdatePatient(Patient patient)
    {
        Patient
    }

    // Delete Patient 
}