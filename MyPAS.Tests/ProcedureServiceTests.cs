using Microsoft.EntityFrameworkCore;
using MyPAS.Data;
using MyPAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MyPAS.Services;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using MyPAS.Models.DTO;
using Microsoft.EntityFrameworkCore;


namespace MyPAS.Tests
{
    public class ProcedureServiceTests
    {
        // Service to Mock
        private readonly Mock<ILogger<PatientService>> _loggerPatientMock;
        private readonly Mock<ILogger<ProcedureService>> _loggerProcedureMock;
        private MyPASContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MyPASContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            return new MyPASContext(options);
        }

        public ProcedureServiceTests()
        {
            _loggerPatientMock = new Mock<ILogger<PatientService>>();
            _loggerProcedureMock = new Mock<ILogger<ProcedureService>>();
        }

        [Fact]
        public async Task CreateServiceForPatientByCreateProcedureDTO_ShouldCreateServiceWithCreateProcedureDTO()
        {
            //Arrange
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context, _loggerProcedureMock.Object);

            var createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Jason",
                LastName = "Silvis",
                Age = 18,
                Email = "test@test.com"
            };

            var patientForProcedure = await patService.CreatePatient(createPatientDTO);


            // Act
            var createdProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patientForProcedure.Id,
                ProcedureName = "TestProcedure",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                CptAmount = 50.00m,
                CptCode = "TESTCODE",
                PatientChargedAmount = 100.00m,
            };

            var result = await procService.CreateProcedureForPatientByCreateProcedureDTO(createdProcedureDTO);

            Assert.NotNull(result);

            var procedureInMemory = context.Procedures
            .Include(p => p.Patient)
            .FirstOrDefault(p => p.Id == result.Id);

            // Assert 

            Assert.Equal(createdProcedureDTO.PatientId, result.PatientId);
            Assert.Equal(createdProcedureDTO.ProcedureName, result.ProcedureName);
            Assert.Equal(createdProcedureDTO.ProcedureDate, result.ProcedureDate);
            Assert.Equal(createdProcedureDTO.CptAmount, result.CptAmount);
            Assert.Equal(createdProcedureDTO.CptCode, result.CptCode);
            Assert.Equal(createdProcedureDTO.PatientChargedAmount, result.PatientChargedAmount);
            
            Assert.NotNull(procedureInMemory);
            Assert.Equal(patientForProcedure.FirstName, procedureInMemory.Patient.FirstName);
        }

        [Fact]
        public async Task GetAllProceduresForPatientById_ShouldReturnAListOfProceduresWithTheSamePatientId()
        {
            // Arrange
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context, _loggerProcedureMock.Object);

            CreatePatientDTO newCreatePatient = new CreatePatientDTO
            {
                FirstName= "Test",
                LastName= "Test",
                Age = 18,
                Email = "test@test.com"
            };

            CreatePatientDTO patientTwo = new CreatePatientDTO
            {
                FirstName = "Test2",
                LastName = "Test2",
                Age = 18,
                Email = "test@test.com"
            };

            var patientForProcedure = await patService.CreatePatient(newCreatePatient);

            // Act
            for (int i = 0; i < 10; i++)
            {
                CreateProcedureDTO newProcedureDTO = new CreateProcedureDTO
                {
                    PatientId = patientForProcedure.Id,
                    ProcedureName = $"TestProcedureName{i}",
                    ProcedureDate = DateOnly.FromDateTime(DateTime.Now),
                    PatientChargedAmount = i,
                    CptAmount = i,
                    CptCode = $"TestCPTCode{i}"

                };

                await procService.CreateProcedureForPatientByCreateProcedureDTO(newProcedureDTO);
            }

            var patientTwoForProcedure = await patService.CreatePatient(patientTwo);

            CreateProcedureDTO patientTwoProcedure = new CreateProcedureDTO
            {
                PatientId = patientTwoForProcedure.Id,
                ProcedureName = "TestPatientTwoProcedure",
                ProcedureDate = DateOnly.FromDateTime(DateTime.Now),
                PatientChargedAmount = 100,
                CptAmount = 100,
                CptCode = $"TestCPTCodeForPatientTwo"
            };

            await procService.CreateProcedureForPatientByCreateProcedureDTO(patientTwoProcedure);

            var procedures = await procService.GetAllProceduresForPatientByPatientId(patientForProcedure.Id);
          
            Assert .NotNull(procedures);
            Assert.Equal(9, procedures.Count());
            Assert.All(procedures, p => Assert.Equal(patientForProcedure.Id, p.PatientId));
        }

        [Fact]
        public async Task GetProcedureByProcedureId_ShouldReturnProcedureDTOFromProcedureId()
        {
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context, _loggerProcedureMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patService.CreatePatient(createPatientDTO);
            ProcedureDTO? procedureToFind = null;

            for (int i = 1; i < 10; i++)
            {
                var patientProcedure = new CreateProcedureDTO
                {
                    PatientId = patient.Id,
                    ProcedureName = $"Test{i}",
                    PatientChargedAmount = i,
                    ProcedureDate = DateOnly.FromDateTime(DateTime.Now),
                    CptAmount = i,
                    CptCode = $"Test{i}",
                 
                };

                var createdProcedure = await procService.CreateProcedureForPatientByCreateProcedureDTO(patientProcedure);

                if (i == 1)
                {
                    procedureToFind = createdProcedure;
                }
            }
            Assert.NotNull(procedureToFind);

            var result = await procService.GetProcedureByProcedureId(procedureToFind.Id);

            Assert.NotNull(result);

            Assert.Equal(procedureToFind.Id, result.Id);
            Assert.Equal(procedureToFind.PatientId, result.PatientId);
            Assert.Equal(procedureToFind.ProcedureName, result.ProcedureName);
            Assert.Equal(procedureToFind.ProcedureDate, result.ProcedureDate);
            Assert.Equal(procedureToFind.CptAmount, result.CptAmount);
            Assert.Equal(procedureToFind.CptCode, result.CptCode);
        }

        [Fact]
        public async Task DeleteProcedureByProcedureId_ShouldDeleteProcedureEntityAndReturnBool()
        {
            // Arrange
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context, _loggerProcedureMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName="TestFN",
                LastName="TestLN",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patService.CreatePatient(createPatientDTO);

            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "TestProcName",
                ProcedureDate = DateOnly.FromDateTime(DateTime.Now),
                PatientChargedAmount = 1,
                CptAmount = 1,
                CptCode = "TestCPTCode"
            };

            var procedureToDelete = await procService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
         
            // Act
            var result = await procService.DeleteProcedureByProcedureId(procedureToDelete.Id);



            // Assert
            Assert.True(result);
            Assert.Empty(context.Procedures);
        }

        [Fact]
        public async Task UpdateProcedureByUpdateProcedureDTO_ShouldReturnProcedureDTO()
        {
            // Arrange
            var context = GetInMemoryContext();
            IPatientService patSevice = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context, _loggerProcedureMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patSevice.CreatePatient(createPatientDTO);

            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "TestProcedureName1",
                ProcedureDate = DateOnly.FromDateTime(DateTime.Now),
                PatientChargedAmount = 1,
                CptAmount = 1,
                CptCode = "TESTCPTCODE1"
            };

            var procedureToUpdate = await procService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);

            // Act 
            UpdateProcedureDTO updateProcedureDTO = new UpdateProcedureDTO
            {
               // Id = procedureToUpdate.Id,
                ProcedureName = "TestProcedureName2",
                PatientChargedAmount = 2,
                CptAmount = 2,
                CptCode = "TESTCPTCODE2"
            };

            await procService.UpdateProcedureByUpdateProcedureDTO(procedureToUpdate.Id, updateProcedureDTO);
            var result = await procService.GetProcedureByProcedureId(procedureToUpdate.Id);

            Assert.NotNull(result);

            var inMemoryProcedure = context.Procedures.Include(p => p.Patient).FirstOrDefault(p => p.Id == result.Id);
            Assert.NotNull(inMemoryProcedure);
            Assert.NotNull(inMemoryProcedure.Patient);
            // Assert
         
            Assert.Equal(procedureToUpdate.Id, result.Id);
            Assert.Equal(updateProcedureDTO.ProcedureName, result.ProcedureName);
            Assert.Equal(updateProcedureDTO.PatientChargedAmount, result.PatientChargedAmount);
            Assert.Equal(updateProcedureDTO.CptAmount, result.CptAmount);
            Assert.Equal(updateProcedureDTO.CptCode, result.CptCode);
            Assert.Equal(inMemoryProcedure.Patient.Id, result.PatientId);
        }

        // Edge Cases
        [Fact]
        public async Task CreateProcedureForPatientByCreateProcedureDTO_EmptyProcedureName_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context,_loggerPatientMock.Object);
            IProcedureService procedureService = new ProcedureService(context, _loggerProcedureMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName= "Test",
                LastName= "Test",
                Age=18,
                Email="TestEmail@Test.com",
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.NotNull(patient);

            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "",
                ProcedureDate = DateOnly.Parse("2026-01-01"),
                PatientChargedAmount = 100,
                CptAmount = 100,
                CptCode = "Test",
            };

            var procedure = await procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
            Assert.Null(procedure);

        }

        [Fact]
        public async Task CreateProcedureForPatientByCreateProcedureDTO_EmptyProcedureDate_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procedureService = new ProcedureService(context, _loggerProcedureMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "TestEmail@Test.com",
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.NotNull(patient);

            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "",
                PatientChargedAmount = 100,
                CptAmount = 100,
                CptCode = "Test",
            };

            var procedure = await procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
            Assert.Null(procedure);

        }

        [Fact]
        public async Task CreateProcedureForPatientByCreateProcedureDTO_ZeroPatientChargedAmount_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procedureService = new ProcedureService(context, _loggerProcedureMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "TestEmail@Test.com",
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.NotNull(patient);

            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "",
                ProcedureDate = DateOnly.Parse("2026-01-01"),
                PatientChargedAmount = 0,
                CptAmount = 100,
                CptCode = "Test",
            };

            var procedure = await procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
            Assert.Null(procedure);

        }


    }
}
