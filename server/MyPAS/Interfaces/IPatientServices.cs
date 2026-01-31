using MyPAS.Models;


public interface IPatientService
{
    IEnumerable<Patient> GetAllPatients();
    Patient GetPatientById(int id);
    Patient CreatePatient(string firstName, string lastName);
    void DeletePatient(int id);
    Patient UpdatePatient(Patient patient);
}