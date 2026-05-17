using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MyPAS.Data
{
    public class MyPASContextFactory : IDesignTimeDbContextFactory<MyPASContext>
    {
        public MyPASContext CreateDbContext(string[] args)
        {
            // Load from configuration.
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Instantiate a database build.
            var optionsBuilder = new DbContextOptionsBuilder<MyPASContext>();
            // Using this connection string.
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new MyPASContext(optionsBuilder.Options);
        }
    }
}
