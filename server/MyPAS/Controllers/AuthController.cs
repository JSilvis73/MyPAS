using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyPAS.Data;
using MyPAS.Models;
using MyPAS.Models.Auth;
using Serilog;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MyPAS.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MyPAS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<MyPASUser> _userManager;
        private readonly SignInManager<MyPASUser> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;

        public AuthController(
            UserManager<MyPASUser> userManager,
            SignInManager<MyPASUser> signInManager,
            ILogger<AuthController> logger,
            IJwtService jwtService,
            IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtService = jwtService;
            _authService = authService;
        }

        // Register, Signin, etc. will go here
        [HttpPost("signIn")]
        public async Task<IActionResult> Signin([FromBody] SignInDTO signInDto)
        {
            // Sign in user
            var result = await _authService.SignIn(signInDto);

            return (result.Success)?
                Ok(result):
                Unauthorized(result.Errors);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterRequest request)
        {

            var result = await _authService.Register(request);

            return (result.Success)? 
                    Ok(result): 
                    Unauthorized(result.Errors);

        }

        [Authorize(Roles = "User")]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var result = await _authService.ChangePassword(User, changePasswordDTO);

            return (result.Result) ?
                    Ok(result) :
                    Unauthorized(result.Error);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] AssignRoleDTO assignRoleDTO)
        {
            var result = await _authService.AddUserToRole(assignRoleDTO);
            return (result.Result) ?
                    Ok(result) :
                    Unauthorized(result.Error);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("removeUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] AssignRoleDTO assignRoleDTO)
        {
            var result = await _authService.RemoveUserFromRole(assignRoleDTO);
            return (result.Result) ?
                    Ok(result) :
                    Unauthorized(result.Error);
        }

        [Authorize(Roles = "User")]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            var result = await _authService.UpdateUser(User, updateUserDTO);
            return (result.Result) ?
                Ok(result) :
                Unauthorized(result.Error);
        }

        [Authorize(Roles = "User")]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _authService.GetCurrentUser(User);
            if (result == null) { return NotFound(); }
            return Ok(result);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteUserByEmail")]
        public async Task<IActionResult> DeactivateUserByEmailAdmin([FromQuery] string emailToDelete)
        {
            var result = await _authService.DisableUserByEmailAdmin(emailToDelete);
            return (result) ?
                Ok(new { Message = $"User with email {emailToDelete} deactivated successfully." }) :
                NotFound(new { Message = $"User with email {emailToDelete} not found." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("activateUserAdmin")]
        public async Task<IActionResult> ActivateUserAdmin([FromQuery] string emailToActivate)
        {
            var result = await _authService.ActivateUserAdmin(emailToActivate);
            return (result) ?
                Ok(new { Message = $"User with email {emailToActivate} activated successfully." }) :
                NotFound(new { Message = $"User with email {emailToActivate} not found." });
        }

        [Authorize(Roles = "User")]
        [HttpDelete("deleteSelf")]
        public async Task<IActionResult> DeactivateSelf()
        {
            var result = await _authService.DeactivateSelf(User);
            return (result) ?
                Ok(new { Message = $"User deleted successfully." }) :
                NotFound(new { Message = $"User not found." });
        }

    }
}

