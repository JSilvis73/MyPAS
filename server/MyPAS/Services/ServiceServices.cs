using MyPAS.Models;
using MyPAS.Data;

public class ServiceService : IServiceService
{
    private readonly MyPASContext _context;

    public ServiceService(MyPASContext context)
    {
        _context = context;
    }

    public Service CreateServiceForPatientByPatientId(int patientId, string serviceName, decimal chargeAmt)
    {
        var service = new Service
        {
            PatientId = patientId,
            ServiceName = serviceName,
            ServiceDate = DateOnly.FromDateTime(DateTime.Now),
            PatientChargedAmount = chargeAmt
        };

        _context.Services.Add(service);
        _context.SaveChanges();

        return service;
    }

    public void DeleteServiceById(int id)
    {
        var serviceToRemove = _context.Services.FirstOrDefault(s => s.Id == id);
        if (serviceToRemove == null)
        {
            throw new Exception("Service not found.");
        }

        _context.Services.Remove(serviceToRemove);
        _context.SaveChanges();
        
    }

    public Service GetServiceById(int id)
    {
        return _context.Services.FirstOrDefault(s => s.Id == id)
            ?? throw new Exception("Service does not exist.");
    }
}