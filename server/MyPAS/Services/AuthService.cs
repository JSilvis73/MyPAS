using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MyPAS.Interfaces;
using MyPAS.Models;
using MyPAS.Models.Auth;

namespace MyPAS.Services
{
    public class AuthService : IAuthService
    {
        // Services.
        private readonly ILogger _logger;
        private readonly SignInManager<MyPASUser> _signInManager;
        private readonly UserManager<MyPASUser> _userManager;
        
        // Constructor.
        public AuthService(ILogger logger,SignInManager<MyPASUser> signInManager, UserManager<MyPASUser> userManager ) 
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Endpoints.
        // Register
        public async Task<AuthResult> Register(AuthRegisterRequest registerRequest)
        {
            // Start log.
            _logger.LogInformation("Attempting to register {Email}.", registerRequest.Email);

            // Check user and gather needed credentials.
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
           
            // Check if user exists.
            if (existingUser != null)
            {
                _logger.LogError("User with email: {Email} already exists.", registerRequest.Email);
                return new AuthResult { Success = false, Error = $"User with email: {registerRequest.Email} already exists." };
            }

            // Create user.
            var userToCreate = new MyPASUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName
            };

            // Attempt to create.
            _logger.LogInformation("Attempting to create user:{Email}.", userToCreate.Email);
            var result = await _userManager.CreateAsync(userToCreate,registerRequest.Password);

            // Check result and return response.
            if (result.Succeeded)
            {
                _logger.LogInformation("User: {Email} has been created.", userToCreate.Email);
                return new AuthResult { Success = true };
            }

            // If we are here something failed.
            foreach (var error in result.Errors)
            {
                
            }

            _logger.LogWarning("Registration failed for user: {Email}. Errors: {result.Errors}.", userToCreate.Email, result.Errors);

            return new AuthResult { Success = false, Error = $"Registration failed for user: {userToCreate.Email}. Errors: {result.Errors}." };

        }

        // Sign In.
        public void SignIn(string username, string password)
        {
            _logger.LogInformation($"Attempting to log in using {username}.");
            throw new NotImplementedException();
        }
 

        // Sign Out.
        public void SignOut(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
