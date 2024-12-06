using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PradCat.Api.Models;
using PradCat.Domain.Entities;
using System.Reflection.Emit;

namespace PradCat.Api.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cat> Cats { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Veterinarian> Veterinarians { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AppUser>(entity =>
        {
            entity.HasOne(u => u.Tutor)
                .WithOne()
                .HasForeignKey<AppUser>(u => u.TutorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Veterinarian>(entity =>
        {
            entity.HasData(
                new Veterinarian { Id = 1, Name = "Natalia Oliveira", Specialty = "Oftalmologista"},
                new Veterinarian { Id = 2, Name = "Luana Soares", Specialty = "Oncologista"},
                new Veterinarian { Id = 3, Name = "Rafael Venâncio", Specialty = "Dentista"},
                new Veterinarian { Id = 4, Name = "Samuel Silva", Specialty = "Clínico Geral"}
                );
        });

        base.OnModelCreating(builder);
    }
}
