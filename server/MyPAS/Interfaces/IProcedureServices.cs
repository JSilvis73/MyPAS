using MyPAS.Models;


public interface IProcedureService
{
    // CRUD
    Procedure CreateProcedureForPatientByPatientId(int patientId, string procedureName, decimal chargeAmt);
    IEnumerable<Procedure> GetAllProceduresForPatientById(int patientId);
    Procedure GetProcedureById(int id);
    //Service GetServicesByPatientName(string patientLastName, string patientFirstName);
    Procedure UpdateProcedureById(int ProcedureId, Procedure procedure);
    void DeleteProcedureById(int id);
    

}