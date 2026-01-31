using MyPAS.Models;


public interface IServiceService
{
    // CRUD
    Service GetServiceById(int id);
    //Service GetServiceByPatientName(string patientLastName, string patientFirstName);
    Service CreateServiceForPatientByPatientId(int patientId, string serviceName, decimal chargeAmt);
    void DeleteServiceById(int id);
    //Service UpdateServiceById(int id);

}