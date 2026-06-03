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
        private readonly ILogger<AuthService> _logger;
        private readonly SignInManager<MyPASUser> _signInManager;
        private readonly UserManager<MyPASUser> _userManager;
        private readonly IJwtService _jwtService;

        // Constructor.
        public AuthService(ILogger<AuthService> logger, SignInManager<MyPASUser> signInManager, UserManager<MyPASUser> userManager, IJwtService jwtService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
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
                return new AuthResult
                {
                    Success = false,
                    Errors = new List<string>
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
            var result = await _userManager.CreateAsync(userToCreate, registerRequest.Password);

            // Check result and return response.
            if (result.Succeeded)
            {
                _logger.LogInformation("User: {Email} has been created.", userToCreate.Email);
                
                return new AuthResult 
                { 
                    Success = true
                };
            }

            // Catch errors.
            var errors = result.Errors.Select(e => e.Description).ToList();


            _logger.LogWarning("Registration failed for user: {Email}. Errors: {Errors}.", userToCreate.Email, string.Join(",", errors));

            return new AuthResult { Success = false, Errors = errors };

        }

        // Sign In.
        public async Task<AuthResult> SignIn(SignInDTO signInDTO)
        {
            _logger.LogInformation("Attempting to log in using {username}.", signInDTO.Email);

            // Check if user exists.
            var userToSignIn = await _userManager.FindByEmailAsync(signInDTO.Email);

            // If the user does not exist, return error.
            if (userToSignIn== null)
            {
                _logger.LogWarning("Invalid email or passord.");
                return new AuthResult ()
                {
                    Success=false,
                    Errors = new List<string>()
                    {
                       $"Invalid email or password."
                    }
                };
            }

            // Build user and sign in.
            var signInResult = await _signInManager.PasswordSignInAsync(userToSignIn ,signInDTO.Password,isPersistent:false,lockoutOnFailure:false);

            // If sign in fails return an error.
            if (!signInResult.Succeeded) 
            { 
                _logger.LogWarning("{Email} has failed to sign in.", userToSignIn.Email);
                return new AuthResult() 
                { 
                    Success = false, 
                    Errors = new List<string>()
                    {
                        $"{userToSignIn.Email} failed to sign in."
                    }
                };
            }

            // Sign in succeeded. Issue JWT.
            var token = _jwtService.GenerateToken(userToSignIn.Id, userToSignIn.Email);

            // Sign in.

                _logger.LogInformation("{Email} was signed in.", userToSignIn.Email);
                return new AuthResult() 
                { 
                    Success = true,
                    Token = token,
                    User = new UserDTO
                    {
                        Email = userToSignIn.Email,
                        FirstName = userToSignIn.FirstName,
                        LastName = userToSignIn.LastName,
                    }
                };

        }

        // Refresh JWT Endpoint.

        // Change Password Endpoint.

        // Assign Roles Endpoint.

    }
}
