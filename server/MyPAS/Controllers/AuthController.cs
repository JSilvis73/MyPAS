using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyPAS.Data;
using MyPAS.Models;
using MyPAS.Models.Auth;
using Serilog;

namespace MyPAS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<MyPASUser> _userManager;
        private readonly SignInManager<MyPASUser> _signInManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<MyPASUser> userManager,
            SignInManager<MyPASUser> signInManager,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // Register, Login, etc. will go here
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Attempting to login user: {email}.", loginDto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {email} logged in successfully.", loginDto.Email);
                return Ok(new { message = "Login successful" });
            }

            _logger.LogWarning("Login failed for user: {email}.", loginDto.Email);
            return Unauthorized(new { message = "Invalid login attempt" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Log information to file.
            _logger.LogInformation("Attempting to register user: {email}.", request.Email);

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            Log.Information("Registering user {email} with first name {firstName} and last name {lastName}.",
                request.Email, request.FirstName, request.LastName);

            // Build new user in memory.
            var user = new MyPASUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,

            };

            // Attempt to create user.
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {email} successfully registered.", request.Email);
                return Ok(new { message = "User registered successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
                // _logger.LogWarning("Registration error for {email}: {error}", request.Email, error.Description);
            }

            return BadRequest(ModelState);

        }


    }

}

