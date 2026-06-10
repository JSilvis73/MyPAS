using MyPAS.Models;
using MyPAS.Models.DTO;


public interface IProcedureService
{
    // CRUD
    Task<ProcedureDTO> CreateProcedureForPatientByCreateProcedureDTO(CreateProcedureDTO createProcedureDTO);
    Task<List<ProcedureDTO>> GetAllProceduresForPatientById(int patientId);
    Task<ProcedureDTO> GetProcedureByProcedureId(int id);
    //Service GetServicesByPatientName(string patientLastName, string patientFirstName);
    Task<ProcedureDTO> UpdateProcedureByUpdateProcedureDTO(UpdateProcedureDTO updateProcedureDTO);
    Task<bool> DeleteProcedureByProcedureId(int id);
    

}