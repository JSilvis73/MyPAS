using MyPAS.Models;


public interface IPatientService
{
    Task<Patient> CreatePatient(string firstName, string lastName);
    Task<List<Patient>> GetAllPatients();
    Task<Patient> GetPatientById(int id);
    Task<Patient> UpdatePatient(Patient patient);
    Task DeletePatient(int id);
    
}