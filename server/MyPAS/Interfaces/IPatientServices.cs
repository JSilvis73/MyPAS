using MyPAS.Models;

public interface IPatientService
{
    IEnumerable<Patient> GetAll();
    Patient GetById(int id);
    Patient CreatePatient(IPatientService patient);
    void DeletePatient(int id);
}