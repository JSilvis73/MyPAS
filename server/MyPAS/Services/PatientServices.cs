//using MyPAS.Models;

//public class PatientServices //: IPatientServices
//{
//    MyPASContext _context;

//    public PatientServices(MyPASContext context)
//    {
//        _context = context;
//    }


//    // Find All Patients

//    // Find Patient by Id
//    public Patient GetPatientById(int id)
//    {
//        var patient = _context.Patients.Find(id);
//        if (patient == null) { return null; }
//        return patient;
//    }
//    // Create Patient
//    public Patient CreatePatient(Patient patient)
//    {
//        _context.Patients.Add(patient);
//        return patient;
//    }

//    // Update Patient
//    public Patient UpdatePatient(Patient patient)
//    {
//        Patient patientToUpdate = _context.Patients.Find(patient.Id);
//        if (patientToUpdate == null) { return patient; }

//        patientToUpdate.FirstName = patient.FirstName;
//        patientToUpdate.LastName = patient.LastName;
//        //patientToUpdate.DateOfBirth = patient.DateOfBirth;
//        patientToUpdate.Address = patient.Address;
//        patientToUpdate.City = patient.City;
//        patientToUpdate.State = patient.State;
//        patientToUpdate.Zip = patient.Zip;
//        patientToUpdate.Age = patient.Age;
//        patientToUpdate.Phone = patient.Phone;
//        patientToUpdate.Email = patient.Email;

//        _context.Patients.Update(patientToUpdate);
//        _context.SaveChanges();

//        return patientToUpdate;
//    }

//    // Delete Patient 
//    public void DeletePatient(int id)
//    {
//        Patient patientToDelete = _context.Patients.Find(id);
//        if (patientToDelete == null) { return; }

//        // Logic to handle related Services and Payments if necessary
//        var relatedServices = _context.Services.Where(s => s.PatientId == id);
//        _context.Services.RemoveRange(relatedServices);

//        var relatedPayments = _context.Payments.Where(p => p.PatientId == id);
//        _context.Payments.RemoveRange(relatedPayments);

//        _context.Patients.Remove(patientToDelete);
//        _context.SaveChanges();
//    }
//}