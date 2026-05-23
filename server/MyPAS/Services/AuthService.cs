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
        public async Task<AuthResult> Register(AuthRegisterRequest registerRequest)
        {
            // Start log.
            _logger.LogInformation($"Attempting to register {registerRequest.Email}.");

            // Build user and gather needed credentials.
            var user = await _userManager.FindByEmailAsync(registerRequest.Email);
            var userEmail = user?.Email;

            // Check user and credentials.
            if (user == null || !await _userManager.CheckPasswordAsync(user, registerRequest.Password) || userEmail == null) 
            {
                // Log error.
                _logger.LogWarning($"Login failed for user: {registerRequest.Email}.");
                // Return unauthorized.
                return new AuthResult { Success = false, Error = "Username and password do not match.", Token = null };
                
            }
            




            throw new NotImplementedException();
        }
        public void LogIn(string username, string password)
        {
            _logger.LogInformation($"Attempting to log in using {username}.");
            throw new NotImplementedException();
        }
        public void LogOut(string username, string password) 
        {
            _logger.LogInformation($"Attempting to log out {username}.");
            throw new NotImplementedException();
        }
    }
}
