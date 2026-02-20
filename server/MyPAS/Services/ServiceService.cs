using MyPAS.Models;
using MyPAS.Data;

namespace MyPAS.Services
{
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
                ServiceDate = DateOnly.FromDateTime(DateTime.Now), // This will need changed to accept input.
                PatientChargedAmount = chargeAmt
            };

            _context.Services.Add(service);
            _context.SaveChanges();

            return service;
        }
        public IEnumerable<Service> GetAllServicesForPatientById(int patientId)
        {
            var services = _context.Services.Where(s => s.PatientId == patientId).ToList();
            if (!services.Any()) { throw new InvalidOperationException("No services for this patient."); }
            return services;
        }

        public Service GetServiceById(int id)
        {
            return _context.Services.FirstOrDefault(s => s.Id == id)
                ?? throw new Exception("Service does not exist.");
        }

        public Service UpdateServiceById(int id, Service service)
        {
            // Find service
            var serviceToUpdate = _context.Services.FirstOrDefault(s => s.Id == id)
            ?? throw new InvalidOperationException("Service does not exist with that id.");

            // Update service
            serviceToUpdate.PatientId = service.PatientId;
            serviceToUpdate.ServiceName = service.ServiceName;
            serviceToUpdate.ServiceDate = service.ServiceDate;
            serviceToUpdate.PatientChargedAmount = service.PatientChargedAmount;
            serviceToUpdate.CptCode = service.CptCode;
            serviceToUpdate.CptAmount = service.CptAmount;

            // Save changes to DB
            _context.SaveChanges();

            return serviceToUpdate;
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
    }
}
