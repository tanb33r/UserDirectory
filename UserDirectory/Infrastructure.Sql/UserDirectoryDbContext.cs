using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Sql;

public class UserDirectoryDbContext : DbContext
{
    public UserDirectoryDbContext(DbContextOptions<UserDirectoryDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Contact)
            .WithOne(c => c.User)
            .HasForeignKey<Contact>(c => c.UserId);

        modelBuilder.Entity<User>()
            .Property(u => u.Sex)
            .HasConversion<string>()
            .HasMaxLength(1);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "staff" },
            new Role { Id = 2, Name = "manager" },
            new Role { Id = 3, Name = "senior swe" },
            new Role { Id = 4, Name = "swe" }
        );

        modelBuilder.Entity<User>().HasData(
            new
            {
                Id = 1,
                FirstName = "Tanvir",
                LastName = "Taher",
                Active = true,
                Company = "Enosis",
                Sex = Sex.M,
                RoleId = 2
            },
            new
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Active = true,
                Company = "Google",
                Sex = Sex.F,
                RoleId = 4
            }
        );

        modelBuilder.Entity<Contact>().HasData(
            new
            {
                Id = 1,
                UserId = 1,
                Phone = "+41023658",
                Address = "Banani",
                City = "Dhaka",
                Country = "Bangladesh"
            },
            new
            {
                Id = 2,
                UserId = 2,
                Phone = "+41023658",
                Address = "Gulshan",
                City = "Dhaka",
                Country = "Bangladesh"
            }
        );
    }
}