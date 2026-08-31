using MyPAS.Models;
using MyPAS.Models.DTO;
using MyPAS.Models.Enums;


public interface IProcedureService
{
    // CRUD
    Task<ProcedureDTO> CreateProcedureForPatientByCreateProcedureDTO(CreateProcedureDTO createProcedureDTO);
    Task<List<ProcedureDTO>> GetAllProceduresForPatientByPatientId(int patientId);
    Task<ProcedureDTO?> GetProcedureByProcedureId(int id);
    //Service GetServicesByPatientName(string patientLastName, string patientFirstName);
    Task<ProcedureDTO> UpdateProcedureByUpdateProcedureDTO(int procedureId, UpdateProcedureDTO updateProcedureDTO);
    Task<DeleteProcedureResult> DeleteProcedureByProcedureId(int id);
    

}