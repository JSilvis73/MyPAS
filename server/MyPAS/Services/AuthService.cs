using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using MyPAS.Interfaces;
using MyPAS.Models;

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
        public void Register(string username, string password)
        {
            _logger.LogInformation("Attempting to register.");
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
