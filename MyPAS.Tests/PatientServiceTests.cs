using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Services;
using MyPAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using MyPAS.Models.DTO;
using Microsoft.AspNetCore.Components.Forms;


namespace MyPAS.Tests
{
    public class PatientServiceTests
    {
        // Services
        private readonly Mock<ILogger<PatientService>> _loggerMock;

        // Mock DB
        private MyPASContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MyPASContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            return new MyPASContext(options);
        }

        public PatientServiceTests()
        {
            // Mock Service
            _loggerMock = new Mock<ILogger<PatientService>>();

        }
 

        [Fact]
        public async Task CreatePatient_ShouldAddPatient()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = _loggerMock.Object;
            IPatientService patService = new PatientService(context, logger);

            var createPatientDTO = new CreatePatientDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                Address = "TestAddress",
                City = "TestCity",
                State = "TestState",
                Zip = "TestZip",
                Age = 18,
                Phone = "3301234567",
                Email = "test@test.com"
            };


            // Act
            var patient = await patService.CreatePatient(createPatientDTO);
            var patInMemory = context.Patients.FirstOrDefault(p => p.Id == patient.Id);

            // Assert
            // Patient
            Assert.NotNull(patient);
            Assert.Equal("TestFN", patient.FirstName);
            Assert.Equal("TestLN", patient.LastName);
            Assert.Equal("TestAddress", patient.Address);
            Assert.Equal("TestCity", patient.City);
            Assert.Equal("TestState", patient.State);
            Assert.Equal("TestZip", patient.Zip);
            Assert.Equal(18, patient.Age);
            Assert.Equal("3301234567", patient.Phone);
            Assert.Equal("test@test.com", patient.Email);

            // DB
            Assert.Single(context.Patients);
            Assert.NotEmpty(context.Patients);
            Assert.NotNull(patInMemory);

            // Patient from db
            Assert.Equal("TestFN", patInMemory.FirstName);
            Assert.Equal("TestLN", patInMemory.LastName);
            Assert.Equal("TestAddress", patInMemory.Address);
            Assert.Equal("TestCity", patInMemory.City);
            Assert.Equal("TestState", patInMemory.State);
            Assert.Equal("TestZip", patInMemory.Zip);
            Assert.Equal(18, patInMemory.Age);
            Assert.Equal("3301234567", patInMemory.Phone);
            Assert.Equal("test@test.com", patInMemory.Email);
        }

        [Fact]
        public async Task GetPatientById_ReturnsPatientWithMatchingId()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = _loggerMock.Object;
            IPatientService patService = new PatientService(context, logger);

            var createPatientDTO = new CreatePatientDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                Address = "TestAddress",
                City = "TestCity",
                State = "TestState",
                Zip = "TestZip",
                Age = 18,
                Phone = "3301234567",
                Email = "test@test.com"
            };

            // Act
            var createdPatient = await patService.CreatePatient(createPatientDTO);
            var result = await patService.GetPatientById(createdPatient.Id);

            // Assert
            Assert.NotNull(createdPatient);
            Assert.Equal(createdPatient.FirstName, result.FirstName);
            Assert.Equal(createdPatient.LastName, result.LastName);
            Assert.Equal(createdPatient.Address, result.Address);
            Assert.Equal(createdPatient.City, result.City);
            Assert.Equal(createdPatient.State, result.State);
            Assert.Equal(createdPatient.Zip, result.Zip);
            Assert.Equal(createdPatient.Age, result.Age);
            Assert.Equal(createdPatient.Phone, result.Phone);
            Assert.Equal(createdPatient.Email, result.Email);
           
        }


        [Fact]
        public async Task GetAllPatients_RetrieveAllPatients()
        {
            var context = GetInMemoryContext();
            var logger = _loggerMock.Object;
            IPatientService service = new PatientService(context, logger);
            

            for (int i = 0; i < 10; i++)
            {
                var createdPatientDTO = new CreatePatientDTO
                {
                    FirstName = $"TestFN{i}",
                    LastName = $"TestLN{i}",
                    Age = 18,
                    Email = $"Test{i}@test.com"
                };

                await service.CreatePatient(createdPatientDTO);
            }

            var list = await service.GetAllPatients();

            Assert.Equal(10, list.Count());

            for (int i = 0; i<10; i++)
            {
                Assert.Contains(list, p => p.FirstName == $"TestFN{i}" && p.LastName == $"TestLN{i}" && p.Age == 18 && p.Email == $"Test{i}@test.com");
            }
        }

        [Fact]
        public async Task UpdatePatient()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = _loggerMock.Object;
            IPatientService patService = new PatientService(context, logger);

            var createPatientDTO = new CreatePatientDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                Address = "TestAddress",
                City = "TestCity",
                State = "TestState",
                Zip = "TestZip",
                Age = 18,
                Phone = "3301234567",
                Email = "Test@test.com"
            };

            // Act
            var patientToUpdate = await patService.CreatePatient(createPatientDTO);

            var updatePatientDTO = new UpdatePatientDTO
            {
                FirstName = "NewTestFN",
                LastName = "NewTestLN",
                Address = "NewTestAddress",
                City = "NewTestCity",
                State = "NewTestState",
                Zip = "NewTestZip",
                Age = 100,
                Phone = "3309990000",
                Email = "NewTest@test.com"
            };

            var result = await patService.UpdatePatient(patientToUpdate.Id, updatePatientDTO);
            var updatedPatientInMemory = context.Patients.FirstOrDefault(p => p.Id == patientToUpdate.Id);
            
            Assert.NotNull(result);

            // Patient from Memory
            Assert.NotNull(updatedPatientInMemory);
            Assert.Equal("NewTestFN", updatedPatientInMemory.FirstName);
            Assert.Equal("NewTestLN", updatedPatientInMemory.LastName);
            Assert.Equal("NewTestAddress", updatedPatientInMemory.Address);
            Assert.Equal("NewTestCity", updatedPatientInMemory.City);
            Assert.Equal("NewTestState", updatedPatientInMemory.State);
            Assert.Equal("NewTestZip", updatedPatientInMemory.Zip);
            Assert.Equal(100, updatedPatientInMemory.Age);
            Assert.Equal("3309990000", updatedPatientInMemory.Phone);
            Assert.Equal("NewTest@test.com", updatedPatientInMemory.Email);
        }

        [Fact]
        public async void DeletePatient_DeletePatientFromDB()
        {
            // Arrange
            var context  = GetInMemoryContext();
            var logger = _loggerMock.Object;
            IPatientService service = new PatientService(context, logger);

            var createPatientDTO = new CreatePatientDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                Address = "TestAddress",
                City = "TestCity",
                State = "TestState",
                Zip = "TestZip",
                Age = 18,
                Phone = "3301234567",
                Email = "Test@test.com"
            };

            // Act
            var patientToDelete = await service.CreatePatient(createPatientDTO);

            var result = await service.DeletePatient(patientToDelete.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(context.Patients);
        }

        // Edge Cases

        [Fact]
        public async Task CreatePatient_EmptyFirstName_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "",
                LastName = "Test",
                Age = 18,
                Phone = "3309991234",
                Email = "TestEmail@test.com"
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.Null(patient);
        }

        [Fact]
        public async Task CreatePatient_EmptyLastName_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "",
                Age = 18,
                Phone = "3309991234",
                Email = "TestEmail@test.com"
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.Null(patient);
        }

        [Fact]
        public async Task CreatePatient_ZeroAge_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 0,
                Phone = "3309991234",
                Email = "TestEmail@test.com"
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.Null(patient);
        }


        [Fact]
        public async Task CreatePatient_EmptyPhoneAndEmail_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Phone = "",
                Email = ""
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.Null(patient);
        }
    }
}
