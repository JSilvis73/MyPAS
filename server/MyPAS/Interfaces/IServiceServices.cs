using MyPAS.Models;


public interface IServiceService
{
    // CRUD
    Service CreateServiceForPatientByPatientId(int patientId, string serviceName, decimal chargeAmt);
    IEnumerable<Service> GetAllServicesForPatientById(int patientId);
    Service GetServiceById(int id);
    //Service GetServicesByPatientName(string patientLastName, string patientFirstName);
    Service UpdateServiceById(int serviceId, Service service);
    void DeleteServiceById(int id);
    

}