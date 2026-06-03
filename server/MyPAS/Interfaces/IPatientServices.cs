using MyPAS.Models;
using MyPAS.Models.DTO;


public interface IPatientService
{
    Task<PatientDTO> CreatePatient(CreatePatientDTO createPatientDTO);
    Task<List<Patient>> GetAllPatients();
    Task<Patient> GetPatientById(int id);
    Task<Patient> UpdatePatient(Patient patient);
    Task DeletePatient(int id);
    
}