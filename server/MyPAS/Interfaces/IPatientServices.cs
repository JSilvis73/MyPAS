using MyPAS.Models;
using MyPAS.Models.DTO;


public interface IPatientService
{
    Task<PatientDTO> CreatePatient(CreatePatientDTO createPatientDTO);
    Task<List<PatientDTO>> GetAllPatients();
    Task<PatientDTO> GetPatientById(int id);
    Task<Patient> UpdatePatient(Patient patient);
    Task<bool> DeletePatient(int id);
    
}