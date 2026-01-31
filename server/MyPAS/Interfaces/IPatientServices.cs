using MyPAS.Models;


public interface IPatientService
{
    IEnumerable<Patient> GetAllPatients();
    Patient GetById(int id);
    Patient CreatePatient(string firstName, string lastName);
    void DeletePatient(int id);
}