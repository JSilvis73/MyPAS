
public interface IServiceService
{
    IEnumerable<Service> GetAllServicesForPatient();
    Service GetServiceById(int id);
    Service CreateService(Service service);
    Service UpdateService(Service service);
    void DeleteServiceById(int id);
    void DeleteServiceByPatient(Patient patient);

}