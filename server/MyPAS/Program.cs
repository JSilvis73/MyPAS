using MyPAS.Data;
using MyPAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

// Begin app builder.
var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddIdentity<MyPASUser, IdentityRole>()
    .AddEntityFrameworkStores<MyPASContext>()
    .AddDefaultTokenProviders();

// Configure Serilog Logger.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Reading from appsettings.
    .MinimumLevel.Information() // Makes the minimum logging level information or highty (error, warning, etc.)
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

// Establishes connection to DB.
builder.Services.AddDbContext<MyPASContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Middleware

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Shows full error trace
}

// Start Logger
Log.Information("MyPAS is starting up...");

// Authentication and Authorization middleware.
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
//app.UseHttpsRedirection();

// Allow Cross-Origin Resource Sharing.
app.UseCors("AllowReactApp");

// Directs HTTP requests as HTTPS.
app.UseAuthorization();

// Establishes endpoints for controllers.
app.MapControllers();

// Start.
app.Run();
