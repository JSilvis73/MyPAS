using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MyPAS.Data;
using MyPAS.Interfaces;
using MyPAS.Models.Auth;
using MyPAS.Services;
using MyPAS.Models;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;

namespace MyPAS.Tests
{
    public class AuthServiceTests
    {
        // Services.
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<UserManager<MyPASUser>> _userManagerMock;
        private readonly Mock<SignInManager<MyPASUser>> _signInManagerMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;

        private readonly AuthService _authService;


        // Construct.
        public AuthServiceTests()
        {
            // Mock services.
            _jwtServiceMock = new Mock<IJwtService>();
            _loggerMock = new Mock<ILogger<AuthService>>();

            // UserManager + SignInManager are more complex (we simplify setup below)
            var store = new Mock<IUserStore<MyPASUser>>();

            // Construct the services.
            _userManagerMock = new Mock<UserManager<MyPASUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<MyPASUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<MyPASUser>>(),
                null, null, null, null);

            _jwtServiceMock
                .Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("fake-token");

            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(),
                null, null, null, null);

            _authService = new AuthService(
           _loggerMock.Object,
           _signInManagerMock.Object,
           _userManagerMock.Object,
           _jwtServiceMock.Object,
           _roleManagerMock.Object
       );
        }

        [Fact]
        public async Task Register_ShouldRegisterUserUsingRegisterRequestDTO()
        {
            // Arrange
            var testRegisterRequest = new AuthRegisterRequest() { Email="TestEmail2026@sample.com", Password="TestPassword1!" };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((MyPASUser)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<MyPASUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);



            // Act
            var result = await _authService.Register(testRegisterRequest);
            var user = await _userManagerMock.Object.FindByEmailAsync(testRegisterRequest.Email);

            // Assert
            Assert.True(result.Success);

            _userManagerMock.Verify(x =>
                x.CreateAsync(It.IsAny<MyPASUser>(), testRegisterRequest.Password),
                Times.Once);
        }

        [Fact]
        public async Task SignIn_Success_ReturnsSuccessResult()
        {
            // Arrange
            var signInDTO = new SignInDTO() 
            {
                Email= "TestEmail2026@sample.com", 
                Password= "TestPassword1!" 
            };

            var user = new MyPASUser()
            {
                Id="123",
                Email=signInDTO.Email,
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    user,
                    signInDTO.Password,
                    false,
                    false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _authService.SignIn(signInDTO);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Token);
        }
    }
}
