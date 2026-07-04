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
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor.
        public AuthService(ILogger<AuthService> logger, SignInManager<MyPASUser> signInManager, UserManager<MyPASUser> userManager, IJwtService jwtService, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
        }

        // Endpoints.
        // Register
        public async Task<AuthResult> Register(AuthRegisterRequest registerRequest)
        {
            if (string.IsNullOrWhiteSpace(registerRequest.Email) || string.IsNullOrWhiteSpace(registerRequest.Password)) 
                return new AuthResult { Success = false, Errors = new List<string> { "Email and password are required." } };

            // Start log.
            _logger.LogInformation("Attempting to register {Email}.", registerRequest.Email);

            // Check user and gather needed credentials.
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);

            // Check if user exists.
            if (existingUser != null)
            {
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
            var result = await _userManager.CreateAsync(userToCreate, registerRequest.Password);

            // Check result and return response.
            if (result.Succeeded)
            {   
                return new AuthResult 
                { 
                    Success = true
                };
            }

            // Catch errors.
            var errors = result.Errors.Select(e => e.Description).ToList();

            return new AuthResult { Success = false, Errors = errors };

        }

        // Sign In.
        public async Task<AuthResult> SignIn(SignInDTO signInDTO)
        {
            _logger.LogInformation("Attempting to log in using {username}.", signInDTO.Email);

            // Check if user exists.
            var userToSignIn = await _userManager.FindByEmailAsync(signInDTO.Email);

            // If the user does not exist, return error.
            if (userToSignIn== null || string.IsNullOrWhiteSpace(userToSignIn.Email))
            {
                return new AuthResult ()
                {
                    Success=false,
                    Errors = new List<string>()
                    {
                       "Invalid email or password."
                    }
                };
            }

            // Build user and sign in.
            var signInResult = await _signInManager.PasswordSignInAsync(userToSignIn ,signInDTO.Password,isPersistent:false,lockoutOnFailure:false);

            // If sign in fails return an error.
            if (!signInResult.Succeeded) 
            { 
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
                return new AuthResult() 
                { 
                    Success = true,
                    Token = token,
                    User = new UserDTO
                    {
                        Id = userToSignIn.Id,
                        Email = userToSignIn.Email,
                        FirstName = userToSignIn.FirstName,
                        LastName = userToSignIn.LastName,
                    }
                };
        }

        // Get user by Email
        public async Task<UserDTO?> GetUserDTOByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        
        // Change Password Endpoint.
        public async Task<AuthIdentityResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            if (string.IsNullOrWhiteSpace(changePasswordDTO.Email) ||
                string.IsNullOrWhiteSpace(changePasswordDTO.Password) || 
                string.IsNullOrWhiteSpace(changePasswordDTO.NewPassword)) 
            { return new AuthIdentityResult { Result = false, Error = { "All fields are required." } }; }

            var user = await _userManager.FindByEmailAsync(changePasswordDTO.Email);
            if (user == null) { return new AuthIdentityResult { Result = false, Error = { "User not found." } }; }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.Password, changePasswordDTO.NewPassword);
            if (!result.Succeeded) { return new AuthIdentityResult { Result = false , Error = { "Unable to change password." } }; }
            return new AuthIdentityResult { Result = true,};
            
        }

        // Assign Roles Endpoint.
        public async Task<AuthIdentityResult> AddUserToRole(AssignRoleDTO assignRoleDTO)
        {
            if (string.IsNullOrWhiteSpace(assignRoleDTO.Email) ||
             string.IsNullOrWhiteSpace(assignRoleDTO.Role))
            {
                return new AuthIdentityResult
                {
                    Result = false,
                    Error = new List<string>
                     {
                      "Email and role are required."
                     }
                };
            }

            var user = await _userManager.FindByEmailAsync(assignRoleDTO.Email);

            if (user == null)
            {
                return new AuthIdentityResult
                {
                    Result = false,
                    Error = new List<string>
                     {
                        "User not found."
                     }
                };
            }

            if (!await _roleManager.RoleExistsAsync(assignRoleDTO.Role))
            {
                return new AuthIdentityResult
                {
                    Result = false,
                    Error = new List<string>
                     {
                      $"Role '{assignRoleDTO.Role}' does not exist."
                     }
                };
            }

            if (await _userManager.IsInRoleAsync(user, assignRoleDTO.Role)) {
                return new AuthIdentityResult
                {
                    Result = false,
                    Error = new List<string>
                     {
                       $"User is already assigned to the '{assignRoleDTO.Role}' role."
                     }
                };
            }

            var identityResult = await _userManager.AddToRoleAsync(user, assignRoleDTO.Role);

            if (!identityResult.Succeeded)
            {
                return new AuthIdentityResult
                {
                    Result = false,
                    Error = identityResult.Errors
                                          .Select(e => e.Description)
                                          .ToList()
                };
            }

            return new AuthIdentityResult
            {
                Result = true
            };
        }

        public async Task<AuthIdentityResult> RemoveUserFromRole(AssignRoleDTO assignRoleDTO)
        {
            // Check DTO Fields.
            if (string.IsNullOrWhiteSpace(assignRoleDTO.Email) ||
                string.IsNullOrWhiteSpace(assignRoleDTO.Role)) 
            { 
                return new AuthIdentityResult { Result = false, Error = { "Email and role must be populated." } }; 
            }
                
            // Get user. Return false if not found.
            var user = await _userManager.FindByEmailAsync(assignRoleDTO.Email);

            if (user == null) { return new AuthIdentityResult { Result = false, Error = new List<string> { "User not found." } }; }

            // Return false if role does not exist.
            if (!await _roleManager.RoleExistsAsync(assignRoleDTO.Role)) 
            { 
                return new AuthIdentityResult 
                { 
                    Result = false, 
                    Error = new List<string> 
                    { "Role does not exist." } 
                }; 
            }

     

            var identityResult = await _userManager.RemoveFromRoleAsync(user, assignRoleDTO.Role);

            if (!identityResult.Succeeded)
            {
                return new AuthIdentityResult
                {
                    Result = false,
                    Error = identityResult.Errors
                                           .Select(e => e.Description)
                                           .ToList()
                };
            }

            return new AuthIdentityResult
            {
                Result = true,
            };
        }
    }
}
