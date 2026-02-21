using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPAS.Models;

namespace MyPAS.Data
{
    // Establish a "Identity" database.
    public class MyPASContext : IdentityDbContext<MyPASUser>
    {
        public MyPASContext(DbContextOptions<MyPASContext> options) : base(options) { }

        // Models for the application.
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Authentication/Authorization Tables
            base.OnModelCreating(modelBuilder);

            // Dates (DateOnly conversions)
            modelBuilder.Entity<Procedure>()
                .Property(p => p.ProcedureDate)
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v))
                .HasColumnType("date");

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentDate)
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v))
                .HasColumnType("date");

            // Payment -> Patient (many-to-one)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment -> Service (many-to-one)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Procedure)
                .WithMany()
                .HasForeignKey(p => p.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);

            // Set decimal precision
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Procedure>()
                .Property(p => p.CptAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Procedure>()
                .Property(p => p.PatientChargedAmount)
                .HasPrecision(18, 2);
        }
    }
}
