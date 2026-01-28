using MyPAS.Models;


public interface IPatientService
{
    IEnumerable<Patient> GetAll();
    Patient GetById(int id);
    Patient CreatePatient(string firstName, string lastName);
    void DeletePatient(int id);
}