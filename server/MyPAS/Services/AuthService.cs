using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyPAS.Data;
using MyPAS.Interfaces;
using MyPAS.Models;
using MyPAS.Models.Auth;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyPAS.Services
{
    public class AuthService : IAuthService
    {
        // Services.
        private readonly ILogger _logger;
        private readonly SignInManager<MyPASUser> _signInManager;
        private readonly UserManager<MyPASUser> _userManager;
        private readonly MyPASContext _context;
        
        // Constructor.
        public AuthService(ILogger logger,SignInManager<MyPASUser> signInManager, UserManager<MyPASUser> userManager, MyPASContext context ) 
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
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
                _logger.LogWarning("User with email: {Email} already exists.", registerRequest.Email);
                return new AuthResult { Success = false, Errors = new List<string>
    {
        $"User with email: {registerRequest.Email} already exists."
    }
                };
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

            // Catch errors.
            var errors = result.Errors.Select(e => e.Description).ToList();
    

            _logger.LogWarning("Registration failed for user: {Email}. Errors: {Errors}.", userToCreate.Email, String.Join(",",errors));

            return new AuthResult { Success = false, Errors = errors };

        }

        // Sign In.
        public void SignIn(string username, string password)
        {
            _logger.LogInformation("Attempting to log in using {username}.", username);

            // Check if user is signed in.
            bool authSignInResult = _signInManager.IsSignedIn(username);
            
            if (_signInManager.IsSignedIn)
            {

            }


            throw new NotImplementedException();
        }
 

        // Sign Out.
        public void SignOut(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
