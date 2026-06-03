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


    }

}

