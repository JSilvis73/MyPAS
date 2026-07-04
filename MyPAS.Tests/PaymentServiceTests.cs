using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MyPAS.Data;
using MyPAS.Interfaces;
using MyPAS.Models.DTO;
using MyPAS.Services;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPAS.Tests
{
    public class PaymentServiceTests
    {

        private readonly Mock<ILogger<PatientService>> _loggerPatientMock;
        private readonly Mock<ILogger<ProcedureService>> _loggerProcedureMock;
        private readonly Mock<ILogger<PaymentService>> _loggerPaymentMock;
        private MyPASContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MyPASContext>()
       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            return new MyPASContext(options);
        }

        public PaymentServiceTests()
        { 
            _loggerPatientMock = new Mock<ILogger<PatientService>>();
            _loggerProcedureMock = new Mock<ILogger<ProcedureService>>();
            _loggerPaymentMock = new Mock<ILogger<PaymentService>>();
        }


        [Fact]
        public async Task CreatePaymentWithCreatePaymentDTO_ShouldReturnPaymentDTO()
        {
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context,_loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context,_loggerProcedureMock.Object);
            IPaymentService payService = new PaymentService(context,_loggerPaymentMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO()
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patService.CreatePatient(createPatientDTO);
             
            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "Test",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 100,
                CptAmount = 100,
                CptCode = "Test",
            };

            var procedure = await procService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);

            // Act
            CreatePaymentDTO createPaymentDTO = new CreatePaymentDTO
            {
                PatientId = patient.Id,
                ProcedureId = procedure.Id,
                Amount = 100,
                Method = "TestVisa",
                PaymentDate = DateOnly.Parse("2026-03-05")
            };

            var payment = await payService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);

            // Check PaymentDTO returned.
            Assert.NotNull(payment);
            Assert.Equal(createPaymentDTO.Amount, payment.Amount);
            Assert.Equal(createPaymentDTO.Method, payment.Method);
            Assert.Equal(createPaymentDTO.PaymentDate, payment.PaymentDate);

            // Check relations.
            var inMemoryPayment = context.Payments.Include(p => p.Patient).Include(p => p.Procedure).FirstOrDefault(p => p.Id == payment.Id);
            Assert.NotNull(inMemoryPayment);
            Assert.NotNull(inMemoryPayment.Patient);
            Assert.Equal(createPatientDTO.FirstName, inMemoryPayment.Patient.FirstName);
            Assert.Equal(createPatientDTO.LastName, inMemoryPayment.Patient.LastName);
            Assert.NotNull(inMemoryPayment.Procedure);
            Assert.Equal(createProcedureDTO.ProcedureName, inMemoryPayment.Procedure.ProcedureName);

        }

        [Fact]
        public async Task GetPaymentByPaymentId_ShouldReturnPaymentDTO()
        {
            // Arrange
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context,_loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context,_loggerProcedureMock.Object);
            IPaymentService payService = new PaymentService(context,_loggerPaymentMock.Object);

            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patService.CreatePatient(createPatientDTO);

            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "Test",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 100,
                CptAmount = 100,
                CptCode = "Test",
            };

            var procedure = await procService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);

            CreatePaymentDTO createPaymentDTO = new CreatePaymentDTO
            {
                PatientId = patient.Id,
                ProcedureId = procedure.Id,
                Amount = 100,
                Method = "TestVisa",
                PaymentDate = DateOnly.Parse("2026-03-05")
            };

            // Act
            var payment = await payService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);
            
            //Assert 
            Assert.NotNull(payment);
            
            var paymentToCheck = await payService.GetPaymentByPaymentId(payment.Id);
            // Check returned dto
            Assert.NotNull(paymentToCheck);
            Assert.Equal(createPaymentDTO.Amount, paymentToCheck.Amount);
            Assert.Equal(createPaymentDTO.Method, paymentToCheck.Method);
            Assert.Equal(createPaymentDTO.PaymentDate, paymentToCheck.PaymentDate);

            // Check Relations
            var inMemoryPayment = context.Payments.Include(p => p.Patient).Include(p => p.Procedure).FirstOrDefault(p => p.Id == payment.Id);
            Assert.NotNull(inMemoryPayment);
            Assert.NotNull(inMemoryPayment.Patient);
            Assert.Equal(inMemoryPayment.Patient.FirstName, createPatientDTO.FirstName);
            Assert.Equal(inMemoryPayment.Patient.LastName, createPatientDTO.LastName);
            Assert.NotNull(inMemoryPayment.Procedure);
            Assert.Equal(inMemoryPayment.Procedure.ProcedureName, createProcedureDTO.ProcedureName);
        }

        [Fact]
        public async Task GetAllPaymentsByProcedureId_ShouldReturnListOfPaymentDTO()
        {

            // Services and Moq.
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context, _loggerProcedureMock.Object);
            IPaymentService payService = new PaymentService(context, _loggerPaymentMock.Object);

            // Create patient A to add procedure and payments to.
            CreatePatientDTO patientADTO = new CreatePatientDTO
            {
                FirstName = "TestA",
                LastName = "TestA",
                Age = 18,
                Email = "test@test.com"
            };

            var patientA = await patService.CreatePatient(patientADTO);

            // Create patient B to add procedure and payments to.
            CreatePatientDTO patientBDTO = new CreatePatientDTO
            {
                FirstName = "TestB",
                LastName = "TestB",
                Age = 18,
                Email = "test@test.com"
            };

            var patientB = await patService.CreatePatient(patientBDTO);

            // Create Procedure A.
            CreateProcedureDTO procedureADTO = new CreateProcedureDTO
            {
                PatientId = patientA.Id,
                ProcedureName = "TestA",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 100,
                CptCode = "TestA",
                CptAmount = 100,

            };

            var procedureA = await procService.CreateProcedureForPatientByCreateProcedureDTO(procedureADTO);

            // Create Procedure B.
            CreateProcedureDTO procedureBDTO = new CreateProcedureDTO
            {
                PatientId = patientB.Id,
                ProcedureName = "TestB",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 100,
                CptCode = "TestB",
                CptAmount = 100,

            };

            var procedureB = await procService.CreateProcedureForPatientByCreateProcedureDTO(procedureBDTO);

            // Act
            // Populate payments for procedure A.
            for (int i = 1; i < 10; i++)
            {
                CreatePaymentDTO paymentToAddDTO = new CreatePaymentDTO
                {
                    PatientId = patientA.Id,
                    ProcedureId = procedureA.Id,
                    PaymentDate = DateOnly.Parse("2026-03-05"),
                    Amount = 50,
                    Method=$"Test{i}",
                };

                await payService.CreatePaymentWithCreatePaymentDTO(paymentToAddDTO);
            }

            // Populate payment for procedure B.
            CreatePaymentDTO paymentBToAddDTO = new CreatePaymentDTO
            {
                PatientId = patientB.Id,
                ProcedureId = procedureB.Id,
                PaymentDate = DateOnly.Parse("2026-03-05"),
                Amount = 100,
                Method = $"TestPat2",
            };

            await payService.CreatePaymentWithCreatePaymentDTO(paymentBToAddDTO);

            // Get results.
            var paymentsForProcedureA = await payService.GetAllPaymentsByProcedureId(procedureA.Id);

            // Assert
            Assert.NotEmpty(paymentsForProcedureA);
            Assert.Equal(9, paymentsForProcedureA.Count);
            Assert.All(paymentsForProcedureA, p => Assert.Equal(patientA.Id, p.PatientId));
            Assert.All(paymentsForProcedureA, p => Assert.Equal(50, p.Amount));
        }

        [Fact]
        public async Task DeletePaymentById_ShouldReturnTrueIfDeleted()
        {
            // Services and Moq.
            var context = GetInMemoryContext();
            IPatientService patService = new PatientService(context,_loggerPatientMock.Object);
            IProcedureService procService = new ProcedureService(context,_loggerProcedureMock.Object);
            IPaymentService payService = new PaymentService(context,_loggerPaymentMock.Object);

            // Create Patient.
            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patService.CreatePatient(createPatientDTO);

            // Create Procedure.
            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "Test",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 1,
                CptCode = "Test",
                CptAmount = 1,
            };

            var procedure = await procService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);

            // Create Payment.
            CreatePaymentDTO createPaymentDTO = new CreatePaymentDTO
            {
                PatientId = patient.Id,
                ProcedureId = procedure.Id,
                Amount = 1,
                PaymentDate = DateOnly.Parse("2026-03-05"),
                Method = "TestMethod",
            };

            var payment = await payService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);
            Assert.NotNull(payment);

            // Act.
            var result = await payService.DeletePaymentById(payment.Id);

            // Assert.
            Assert.True(result);
        }

        [Fact]
        public async Task UpdatePaymentByDTO_ShouldReturnPaymentDTO()
        {
            // Services and Moq.
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context,_loggerPatientMock.Object);
            IProcedureService procedureService = new ProcedureService(context,_loggerProcedureMock.Object);
            IPaymentService paymentService = new PaymentService(context,_loggerPaymentMock.Object);

            // Create Patient.
            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
           

            // Create Procedure.
            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "Test",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 1,
                CptCode = "Test",
                CptAmount = 1,
            };

            var procedure = await procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);

            // Create Payment.
            CreatePaymentDTO createPaymentDTO = new CreatePaymentDTO
            {
                PatientId = patient.Id,
                ProcedureId = procedure.Id,
                Amount = 100,
                Method = "TestVisa",
                PaymentDate = DateOnly.Parse("2026-03-05")
            };

            var payment = await paymentService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);

            Assert.NotNull(payment);

            // Act
            UpdatePaymentDTO updatePaymentDTO = new UpdatePaymentDTO
            {
                Amount = 50.50m,
                Method = "TestMC",
                PaymentDate = DateOnly.Parse("2026-05-05"),
            };

            var updateResult = await paymentService.UpdatePaymentByDTO(payment.Id, updatePaymentDTO);
            Assert.NotNull(updateResult);

            // Assert
            Assert.Equal(updatePaymentDTO.Amount, updateResult.Amount);
            Assert.Equal(updatePaymentDTO.Method, updateResult.Method);
            Assert.Equal(updatePaymentDTO.PaymentDate, updateResult.PaymentDate);
            
            // Check FK
            Assert.Equal(createPaymentDTO.PatientId, updateResult.PatientId);
            Assert.Equal(createPaymentDTO.ProcedureId, updateResult.ProcedureId);

            // Check Relations
            var relations = context.Payments.Include(p => p.Patient).Include(p => p.Procedure).FirstOrDefault(p => p.Id == updateResult.Id);
            Assert.NotNull(relations);
            Assert.NotNull(relations.Patient);
            Assert.Equal(patient.Id, relations.Patient.Id);
            Assert.Equal(createPatientDTO.FirstName, relations.Patient.FirstName);
            Assert.NotNull(relations.Procedure);
            Assert.Equal(procedure.Id, relations.Procedure.Id);
            Assert.Equal(procedure.ProcedureName, relations.Procedure.ProcedureName);

        }

        [Fact]
        public async Task CreatePaymentWithCreatePaymentDTO_EmptyMethod_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procedureService = new ProcedureService(context, _loggerProcedureMock.Object);
            IPaymentService paymentService = new PaymentService(context, _loggerPaymentMock.Object);

            // Patient
            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.NotNull(patient);

            // Procedure
            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "Test",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 1,
                CptCode = "Test",
                CptAmount = 1,
            };

            var procedure = await procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
            Assert.NotNull(procedure);

            // Payment
            CreatePaymentDTO createPaymentDTO = new CreatePaymentDTO
            {
                PatientId = patient.Id,
                ProcedureId = procedure.Id,
                Amount = 100,
                Method = "",
                PaymentDate = DateOnly.Parse("2026-03-05")
            };

            var payment = await paymentService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);
            Assert.Null(payment);
        }

        [Fact]
        public async Task CreatePaymentWithCreatePaymentDTO_ZeroPaymentAmount_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procedureService = new ProcedureService(context, _loggerProcedureMock.Object);
            IPaymentService paymentService = new PaymentService(context, _loggerPaymentMock.Object);

            // Patient
            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.NotNull(patient);

            // Procedure
            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "Test",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 1,
                CptCode = "Test",
                CptAmount = 1,
            };

            var procedure = await procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
            Assert.NotNull(procedure);

            // Payment
            CreatePaymentDTO createPaymentDTO = new CreatePaymentDTO
            {
                PatientId = patient.Id,
                ProcedureId = procedure.Id,
                Amount = 0,
                Method = "TestVisa",
                PaymentDate = DateOnly.Parse("2026-03-05")
            };

            var payment = await paymentService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);
            Assert.Null(payment);
        }

        [Fact]
        public async Task CreatePaymentWithCreatePaymentDTO_NoPaymentDate_ShouldReturnNull()
        {
            var context = GetInMemoryContext();
            IPatientService patientService = new PatientService(context, _loggerPatientMock.Object);
            IProcedureService procedureService = new ProcedureService(context, _loggerProcedureMock.Object);
            IPaymentService paymentService = new PaymentService(context, _loggerPaymentMock.Object);

            // Patient
            CreatePatientDTO createPatientDTO = new CreatePatientDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 18,
                Email = "test@test.com"
            };

            var patient = await patientService.CreatePatient(createPatientDTO);
            Assert.NotNull(patient);

            // Procedure
            CreateProcedureDTO createProcedureDTO = new CreateProcedureDTO
            {
                PatientId = patient.Id,
                ProcedureName = "Test",
                ProcedureDate = DateOnly.Parse("2026-03-05"),
                PatientChargedAmount = 1,
                CptCode = "Test",
                CptAmount = 1,
            };

            var procedure = await procedureService.CreateProcedureForPatientByCreateProcedureDTO(createProcedureDTO);
            Assert.NotNull(procedure);

            // Payment
            CreatePaymentDTO createPaymentDTO = new CreatePaymentDTO
            {
                PatientId = patient.Id,
                ProcedureId = procedure.Id,
                Amount = 100,
                Method = "TestVisa",
               // PaymentDate = DateOnly.Parse("2026-03-05")
            };

            var payment = await paymentService.CreatePaymentWithCreatePaymentDTO(createPaymentDTO);
            Assert.Null(payment);
        }
    }
}
