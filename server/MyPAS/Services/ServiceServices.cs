//using MyPAS.Data;
//using MyPAS.Models;

//public class ServiceServices : IServiceServices
//{
//    MyPASContext _context;

//    public ServiceServices(MyPASContext context)
//    {
//        _context = context;
//    }

//    // Find Service by Id
//    public Service GetServiceById(int id)
//    {
//        var service = _context.Services.Find(id);
//        if (service == null) { return null; }

//        return service;
//    }

//}