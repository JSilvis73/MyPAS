using MyPAS.Data;
using MyPAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyPAS.Services;
using MyPAS.Interfaces;

// Begin app builder.
var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddIdentity<MyPASUser, IdentityRole>()
    .AddEntityFrameworkStores<MyPASContext>()
    .AddDefaultTokenProviders();

// Establishes connection to DB.
builder.Services.AddDbContext<MyPASContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key is not configured in appsettings.json");
}

// Configure JWT authentication.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});     

// Configure Serilog Logger.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Reading from appsettings.
    .MinimumLevel.Information() // Makes the minimum logging level information or higher (error, warning, etc.)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Only warn or error for Microsoft logs
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning) // Hide SQL commands
    .WriteTo.Console() 
    .WriteTo.File("Logs/mypas-log.txt", rollingInterval: RollingInterval.Day)   // Outputs to text file with the interval of one per day.
    .CreateLogger();

//Plug Serilog into .NET Core.
builder.Host.UseSerilog(); 

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Your Vite frontend port
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add services to the container.


builder.Services.AddControllers();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IProcedureService, ProcedureService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();


var app = builder.Build();

async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    // Instantiate a serviceProvider.
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Create standard list of roles.
    string[] roles = ["Admin", "User"];

    // Loop over the roles and see if they exist in the database. If it doesn't add it.
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Middleware

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Shows full error trace
}

// Start Logger
Log.Information("MyPAS is starting up...");

// Configure the HTTP request pipeline.
//app.UseHttpsRedirection();

// Allow Cross-Origin Resource Sharing.
app.UseCors("AllowReactApp");

// Authentication and Authorization middleware.
app.UseAuthentication();
app.UseAuthorization();

// Establishes endpoints for controllers.
app.MapControllers();

// Start.
Log.Information("MyPAS has started successfully.");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Seed roles
    await SeedRolesAsync(services);

    // Seed default admin
    await SeedAdminAsync(services);
}

// Method to create one admin user upon startup
async Task SeedAdminAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<MyPASUser>>();

    string adminEmail = "support@mypas.com";
    string adminPassword = "SuperSecure123!"; // change before production

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdmin = new MyPASUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newAdmin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}

app.Run();

