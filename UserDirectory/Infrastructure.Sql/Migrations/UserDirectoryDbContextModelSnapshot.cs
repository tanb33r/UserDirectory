﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserDirectory.Infrastructure.Sql;

#nullable disable

namespace UserDirectory.Infrastructure.Sql.Migrations
{
    [DbContext(typeof(UserDirectoryDbContext))]
    partial class UserDirectoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserDirectory.Domain.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Contacts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Banani",
                            City = "Dhaka",
                            Country = "Bangladesh",
                            Phone = "+41023658",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Address = "Gulshan",
                            City = "Dhaka",
                            Country = "Bangladesh",
                            Phone = "+41023658",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("UserDirectory.Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "staff"
                        },
                        new
                        {
                            Id = 2,
                            Name = "manager"
                        },
                        new
                        {
                            Id = 3,
                            Name = "senior swe"
                        },
                        new
                        {
                            Id = 4,
                            Name = "swe"
                        });
                });

            modelBuilder.Entity("UserDirectory.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            Company = "Enosis",
                            FirstName = "Tanvir",
                            LastName = "Taher",
                            RoleId = 2,
                            Sex = "M"
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            Company = "Google",
                            FirstName = "Jane",
                            LastName = "Doe",
                            RoleId = 4,
                            Sex = "F"
                        });
                });

            modelBuilder.Entity("UserDirectory.Domain.Contact", b =>
                {
                    b.HasOne("UserDirectory.Domain.User", "User")
                        .WithOne("Contact")
                        .HasForeignKey("UserDirectory.Domain.Contact", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserDirectory.Domain.User", b =>
                {
                    b.HasOne("UserDirectory.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UserDirectory.Domain.User", b =>
                {
                    b.Navigation("Contact")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
