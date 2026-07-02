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
            var request = new AuthRegisterRequest
            {
                Email = "TestEmail2026@sample.com",
                Password = "TestPassword1!"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((MyPASUser)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<MyPASUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            MyPASUser capturedUser = null;

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<MyPASUser>(), request.Password))
                .Callback<MyPASUser, string>((u, p) => capturedUser = u)
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.Register(request);

            // Assert
            Assert.True(result.Success);

            Assert.NotNull(capturedUser);
            Assert.Equal(request.Email, capturedUser.Email);

            _userManagerMock.Verify(x =>
                x.CreateAsync(It.IsAny<MyPASUser>(), request.Password),
                Times.Once);
        }

        [Fact]
        public async Task Register_MissingEmail_ShouldReturnFailureResult()
        {
            // Arrange
            var request = new AuthRegisterRequest
            {
                Email = "",
                Password = "TestPassword1!"
            };

            // Act
            var result = await _authService.Register(request);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Register_MissingPassword_ShouldReturnFailureResult()
        {
            var testRegisterRequest = new AuthRegisterRequest()
            {
                Email = "TestEmail2026@sample.com",
                Password = ""
            };

            var result = await _authService.Register(testRegisterRequest);
            Assert.False(result.Success);
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

        [Fact]
        public async Task SignIn_Failure_ShouldReturnFailedResult()
        {
            var signInDTO = new SignInDTO()
            {
                Email = "TestEmail2026@sample.com",
                Password = ""
            };

            var user = new MyPASUser()
            {
                Id = "123",
                Email = signInDTO.Email,
            };

            _userManagerMock.
                Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(
                    user,
                    signInDTO.Password,
                    false,
                    false))
                .ReturnsAsync(SignInResult.Failed);

            // Act 
            var result = await _authService.SignIn(signInDTO);

            Assert.False(result.Success);
            Assert.Null(result.Token);
        }

        [Fact]
        public async Task GetUserDTOByEmail_Success_ShouldReturnUserDTO()
        {
            var registerRequest = new AuthRegisterRequest()
            {
                Email = "TestEmail@test.com",
                Password = "TestPassword1!"
            };

            var user = new MyPASUser()
            {
                Id = "123",
                Email = registerRequest.Email,
            };

            _userManagerMock.
                Setup(x => x.FindByEmailAsync(registerRequest.Email))
                .ReturnsAsync(user);

            var result = await _authService.GetUserDTOByEmail(registerRequest.Email);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task ChangePassword_Success_ShouldReturnTrue()
        {
            var changePasswordDTO = new ChangePasswordDTO()
            {
                Email = "TestEmail.com",
                Password = "OldPassword1!",
                NewPassword = "NewPassword1!",
                ConfirmPassword = "NewPassword1!"
            };

            var user = new MyPASUser()
            {
                Id = "123",
                Email = changePasswordDTO.Email,
            };
            
            _userManagerMock
                .Setup(x => x.FindByEmailAsync(changePasswordDTO.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ChangePasswordAsync(user, changePasswordDTO.Password, changePasswordDTO.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.ChangePassword(changePasswordDTO);

            Assert.True(result.Result);
        }

        [Fact]
        public async Task ChangePassword_Failure_ShouldReturnFalse()
        {
            var changePasswordDTO = new ChangePasswordDTO()
            {
                Email = "TestEmail@Test.com",
                Password = "OldPassword1!",
                NewPassword = "NewPassword1!",
                ConfirmPassword = "NewPassword1!"
            };

            var user = new MyPASUser()
            {
                Id = "123",
                Email = changePasswordDTO.Email,
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(changePasswordDTO.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ChangePasswordAsync(user, changePasswordDTO.Password, changePasswordDTO.NewPassword))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _authService.ChangePassword(changePasswordDTO);

            Assert.False(result.Result);
            Assert.NotEmpty(result.Error);
        }

        [Fact]
        public async Task AddUserToRole_Success_ShouldAddRoleToUser()
        {
            var user = new MyPASUser()
            {
                Id = "123",
                Email = "TestEmail@Test.com",
            };

            var assignRoleDTO = new AssignRoleDTO()
            {
                Email = user.Email,
                Role = "Admin"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _roleManagerMock
                .Setup(x => x.RoleExistsAsync(assignRoleDTO.Role))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.IsInRoleAsync(user, assignRoleDTO.Role))
                .ReturnsAsync(false);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(user, assignRoleDTO.Role))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.AddUserToRole(assignRoleDTO);

            Assert.NotNull(result);
            Assert.True(result.Result);

        }

        [Fact]
        public async Task RemoveUserFromRole()
        {
            var user = new MyPASUser()
            {
                Id = "123",
                Email = "TestEmail@Test.com"
            };

            var assignRoleDTO = new AssignRoleDTO()
            {
                Email = user.Email,
                Role = "Admin"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _roleManagerMock
                .Setup(x => x.RoleExistsAsync(assignRoleDTO.Role))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.IsInRoleAsync(user, assignRoleDTO.Role))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.RemoveFromRoleAsync(user, assignRoleDTO.Role))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.RemoveUserFromRole(assignRoleDTO);

            Assert.True(result.Result);
        }
    }
}
