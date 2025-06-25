using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyPAS.Data
{
    public class MyPASContextFactory : IDesignTimeDbContextFactory<MyPASContext>
    {
        public MyPASContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyPASContext>();

            optionsBuilder.UseSqlServer("Server=localhost;Database=MyPASDBProd;Trusted_Connection=True;TrustServerCertificate=True;");


            return new MyPASContext(optionsBuilder.Options);
        }
    }
}
