using MyPAS.Data;
using Microsoft.EntityFrameworkCore;

// Begin app builder
var builder = WebApplication.CreateBuilder(args);

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
