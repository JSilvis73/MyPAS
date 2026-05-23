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

namespace MyPAS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<MyPASUser> _userManager;
        private readonly SignInManager<MyPASUser> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtService _jwtService;

        public AuthController(
            UserManager<MyPASUser> userManager,
            SignInManager<MyPASUser> signInManager,
            ILogger<AuthController> logger,
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtService = jwtService;
        }

        // Register, Login, etc. will go here
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Attempting to login user: {email}.", loginDto.Email);

            // Check if login model is valid.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Model is valid, proceed with login.
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var userEmail = user?.Email;

            // Check: user is valid, ensure password matches, and user email is populated.
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password) || userEmail == null)
            {
                _logger.LogWarning("Login failed for user: {email}.", loginDto.Email);
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Assemble the user.
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            // Create claims and sign in the user.
            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed for user: {email}.", loginDto.Email);
                return Unauthorized(new { message = "Invalid login attempt." });
            }

            // Upon login in generate JWT.
            var token = _jwtService.GenerateToken(user.Id, userEmail);

            // If token is valid, issue token.
            if (token != null)
            {
                _logger.LogInformation("User {email} logged in successfully.", loginDto.Email);
                return Ok(new { token, user = new { user.Id, user.Email, user.FirstName, user.LastName } });
            }
               
            // If we got here, login was unsuccessful.
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
                _logger.LogWarning("Invalid registration attempt for user: {email}.", request.Email);
                return BadRequest(ModelState);
            }

            // Model is valid, proceed with registration.
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

            // If we got here, something failed.
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
                // _logger.LogWarning("Registration error for {email}: {error}", request.Email, error.Description);
            }

            _logger.LogWarning("Registration failed for user: {email}. Errors: {errors}", request.Email, result.Errors);

            return BadRequest(ModelState);

        }


    }

}

