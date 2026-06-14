using MyPAS.Models;
using MyPAS.Models.DTO;


public interface IProcedureService
{
    // CRUD
    Task<ProcedureDTO> CreateProcedureForPatientByCreateProcedureDTO(CreateProcedureDTO createProcedureDTO);
    Task<List<ProcedureDTO>> GetAllProceduresForPatientByPatientId(int patientId);
    Task<ProcedureDTO?> GetProcedureByProcedureId(int id);
    //Service GetServicesByPatientName(string patientLastName, string patientFirstName);
    Task<ProcedureDTO> UpdateProcedureByUpdateProcedureDTO(int procedureId, UpdateProcedureDTO updateProcedureDTO);
    Task<bool> DeleteProcedureByProcedureId(int id);
    

}