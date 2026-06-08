using MyPAS.Models;
using MyPAS.Models.DTO;


public interface IProcedureService
{
    // CRUD
    Task<ProcedureDTO> CreateProcedureForPatientByCreateProcedureDTO(CreateProcedureDTO createProcedureDTO);
    Task<List<ProcedureDTO>> GetAllProceduresForPatientById(int patientId);
    Task<ProcedureDTO> GetProcedureByProcedureId(int id);
    //Service GetServicesByPatientName(string patientLastName, string patientFirstName);
    Procedure UpdateProcedureById(int ProcedureId, Procedure procedure);
    void DeleteProcedureById(int id);
    

}