using MyPAS.Models;


public interface IPatientService
{
    Patient CreatePatient(string firstName, string lastName);
    IEnumerable<Patient> GetAllPatients();
    Patient GetPatientById(int id);
    Patient UpdatePatient(Patient patient);
    void DeletePatient(int id);
    
}