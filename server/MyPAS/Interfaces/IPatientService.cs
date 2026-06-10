using MyPAS.Models;
using MyPAS.Models.DTO;


public interface IPatientService
{
    Task<PatientDTO> CreatePatient(CreatePatientDTO createPatientDTO);
    Task<List<PatientDTO>> GetAllPatients();
    Task<PatientDTO> GetPatientById(int id);
    Task<PatientDTO> UpdatePatient(int id, UpdatePatientDTO updatePatientDTO);
    Task<bool> DeletePatient(int id);
    
}