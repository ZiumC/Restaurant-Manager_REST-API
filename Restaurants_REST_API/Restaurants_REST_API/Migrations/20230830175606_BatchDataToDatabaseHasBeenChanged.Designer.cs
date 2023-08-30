﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurants_REST_API.DbContexts;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20230830175606_BatchDataToDatabaseHasBeenChanged")]
    partial class BatchDataToDatabaseHasBeenChanged
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Address", b =>
                {
                    b.Property<int>("IdAddress")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAddress"), 1L, 1);

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LocalNumber")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdAddress");

                    b.ToTable("Address");

                    b.HasData(
                        new
                        {
                            IdAddress = 1,
                            BuildingNumber = "11",
                            City = "Warsaw",
                            LocalNumber = "1",
                            Street = "John Paul II"
                        },
                        new
                        {
                            IdAddress = 2,
                            BuildingNumber = "1",
                            City = "Warsaw",
                            LocalNumber = "2",
                            Street = "John Paul II"
                        },
                        new
                        {
                            IdAddress = 3,
                            BuildingNumber = "78",
                            City = "Warsaw",
                            LocalNumber = "32A",
                            Street = "Stawki"
                        },
                        new
                        {
                            IdAddress = 4,
                            BuildingNumber = "14",
                            City = "Warsaw",
                            Street = "Romualda"
                        },
                        new
                        {
                            IdAddress = 5,
                            BuildingNumber = "B2",
                            City = "Warsaw",
                            LocalNumber = "21",
                            Street = "Skret"
                        },
                        new
                        {
                            IdAddress = 6,
                            BuildingNumber = "A1",
                            City = "Warsaw",
                            Street = "Koszyk"
                        },
                        new
                        {
                            IdAddress = 7,
                            BuildingNumber = "21",
                            City = "Warsaw",
                            LocalNumber = "37",
                            Street = "John Paul II"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Certificate", b =>
                {
                    b.Property<int>("IdCertificate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCertificate"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("nvarchar(125)");

                    b.HasKey("IdCertificate");

                    b.ToTable("Certificate");

                    b.HasData(
                        new
                        {
                            IdCertificate = 1,
                            Name = "Italian Cuisine"
                        },
                        new
                        {
                            IdCertificate = 2,
                            Name = "Germany Cuisine"
                        },
                        new
                        {
                            IdCertificate = 3,
                            Name = "Polish Cuisine"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Client", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdClient"), 1L, 1);

                    b.Property<string>("IsBusinessman")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdClient");

                    b.ToTable("Client");

                    b.HasData(
                        new
                        {
                            IdClient = 1,
                            IsBusinessman = "N",
                            Name = "Jan"
                        },
                        new
                        {
                            IdClient = 2,
                            IsBusinessman = "N",
                            Name = "Michal"
                        },
                        new
                        {
                            IdClient = 3,
                            IsBusinessman = "Y",
                            Name = "Joanna"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Complaint", b =>
                {
                    b.Property<int>("IdComplaint")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdComplaint"), 1L, 1);

                    b.Property<DateTime>("ComplainDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ComplaintMessage")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("ComplaintStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IdReservation")
                        .HasColumnType("int");

                    b.Property<int>("IdRestaurant")
                        .HasColumnType("int");

                    b.HasKey("IdComplaint");

                    b.HasIndex("IdReservation")
                        .IsUnique();

                    b.HasIndex("IdRestaurant");

                    b.ToTable("Complaint");

                    b.HasData(
                        new
                        {
                            IdComplaint = 1,
                            ComplainDate = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ComplaintMessage = "Zupa była za słona, obsługa była niemiła",
                            ComplaintStatus = "REJECTED",
                            IdReservation = 1,
                            IdRestaurant = 1
                        },
                        new
                        {
                            IdComplaint = 2,
                            ComplainDate = new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ComplaintMessage = "Na kotlecie była mucha",
                            ComplaintStatus = "NEW",
                            IdReservation = 3,
                            IdRestaurant = 1
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Dish", b =>
                {
                    b.Property<int>("IdDish")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDish"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("IdDish");

                    b.ToTable("Dish");

                    b.HasData(
                        new
                        {
                            IdDish = 1,
                            Name = "Pizza",
                            Price = 19.99m
                        },
                        new
                        {
                            IdDish = 2,
                            Name = "Calzone",
                            Price = 11.99m
                        },
                        new
                        {
                            IdDish = 3,
                            Name = "Pierogi",
                            Price = 9.99m
                        },
                        new
                        {
                            IdDish = 4,
                            Name = "Brat Wurst",
                            Price = 7.99m
                        },
                        new
                        {
                            IdDish = 5,
                            Name = "Cheese Bread",
                            Price = 19.99m
                        },
                        new
                        {
                            IdDish = 6,
                            Name = "Carbonara",
                            Price = 12.99m
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Employee", b =>
                {
                    b.Property<int>("IdEmployee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmployee"), 1L, 1);

                    b.Property<decimal>("BonusSalary")
                        .HasColumnType("money");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("FirstPromotionChefDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("HiredDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdAddress")
                        .HasColumnType("int");

                    b.Property<string>("IsOwner")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PESEL")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money");

                    b.HasKey("IdEmployee");

                    b.HasIndex("IdAddress");

                    b.ToTable("Employee");

                    b.HasData(
                        new
                        {
                            IdEmployee = 1,
                            BonusSalary = 150m,
                            FirstName = "Michal",
                            FirstPromotionChefDate = new DateTime(2000, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HiredDate = new DateTime(1999, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 3,
                            IsOwner = "Y",
                            LastName = "Nowak",
                            PESEL = "11122233344",
                            Salary = 3421m
                        },
                        new
                        {
                            IdEmployee = 2,
                            BonusSalary = 150m,
                            FirstName = "Kacper",
                            HiredDate = new DateTime(2002, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 4,
                            IsOwner = "N",
                            LastName = "Kawka",
                            PESEL = "22233344455",
                            Salary = 2217m
                        },
                        new
                        {
                            IdEmployee = 3,
                            BonusSalary = 150m,
                            FirstName = "Janusz",
                            HiredDate = new DateTime(2004, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 5,
                            IsOwner = "N",
                            LastName = "Jogurt",
                            PESEL = "33344455566",
                            Salary = 2213m
                        },
                        new
                        {
                            IdEmployee = 4,
                            BonusSalary = 150m,
                            FirstName = "Bozena",
                            HiredDate = new DateTime(2005, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 6,
                            IsOwner = "N",
                            LastName = "Lawenda",
                            PESEL = "44455566677",
                            Salary = 2613m
                        },
                        new
                        {
                            IdEmployee = 5,
                            BonusSalary = 150m,
                            FirstName = "Joanna",
                            HiredDate = new DateTime(2010, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 7,
                            IsOwner = "N",
                            LastName = "Skrzynka",
                            PESEL = "55566677788",
                            Salary = 2913m
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.EmployeeCertificate", b =>
                {
                    b.Property<int>("IdEmployeeCertificate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmployeeCertificate"), 1L, 1);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdCertificate")
                        .HasColumnType("int");

                    b.Property<int>("IdEmployee")
                        .HasColumnType("int");

                    b.HasKey("IdEmployeeCertificate");

                    b.HasIndex("IdCertificate");

                    b.HasIndex("IdEmployee");

                    b.ToTable("EmployeeCertificate");

                    b.HasData(
                        new
                        {
                            IdEmployeeCertificate = 1,
                            ExpirationDate = new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdCertificate = 1,
                            IdEmployee = 1
                        },
                        new
                        {
                            IdEmployeeCertificate = 2,
                            ExpirationDate = new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdCertificate = 1,
                            IdEmployee = 2
                        },
                        new
                        {
                            IdEmployeeCertificate = 3,
                            ExpirationDate = new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdCertificate = 2,
                            IdEmployee = 1
                        },
                        new
                        {
                            IdEmployeeCertificate = 4,
                            ExpirationDate = new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdCertificate = 2,
                            IdEmployee = 4
                        },
                        new
                        {
                            IdEmployeeCertificate = 5,
                            ExpirationDate = new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdCertificate = 2,
                            IdEmployee = 3
                        },
                        new
                        {
                            IdEmployeeCertificate = 6,
                            ExpirationDate = new DateTime(2023, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdCertificate = 3,
                            IdEmployee = 1
                        },
                        new
                        {
                            IdEmployeeCertificate = 7,
                            ExpirationDate = new DateTime(2023, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdCertificate = 3,
                            IdEmployee = 5
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.EmployeeRestaurant", b =>
                {
                    b.Property<int>("IdRestaurantWorker")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRestaurantWorker"), 1L, 1);

                    b.Property<int>("IdEmployee")
                        .HasColumnType("int");

                    b.Property<int>("IdRestaurant")
                        .HasColumnType("int");

                    b.Property<int>("IdType")
                        .HasColumnType("int");

                    b.HasKey("IdRestaurantWorker");

                    b.HasIndex("IdEmployee");

                    b.HasIndex("IdRestaurant");

                    b.HasIndex("IdType");

                    b.ToTable("EmployeeRestaurant");

                    b.HasData(
                        new
                        {
                            IdRestaurantWorker = 1,
                            IdEmployee = 1,
                            IdRestaurant = 1,
                            IdType = 1
                        },
                        new
                        {
                            IdRestaurantWorker = 2,
                            IdEmployee = 2,
                            IdRestaurant = 1,
                            IdType = 2
                        },
                        new
                        {
                            IdRestaurantWorker = 3,
                            IdEmployee = 3,
                            IdRestaurant = 1,
                            IdType = 3
                        },
                        new
                        {
                            IdRestaurantWorker = 4,
                            IdEmployee = 4,
                            IdRestaurant = 1,
                            IdType = 3
                        },
                        new
                        {
                            IdRestaurantWorker = 5,
                            IdEmployee = 5,
                            IdRestaurant = 1,
                            IdType = 4
                        },
                        new
                        {
                            IdRestaurantWorker = 6,
                            IdEmployee = 1,
                            IdRestaurant = 2,
                            IdType = 1
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.EmployeeType", b =>
                {
                    b.Property<int>("IdType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdType"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdType");

                    b.ToTable("EmployeeType");

                    b.HasData(
                        new
                        {
                            IdType = 1,
                            Name = "Owner"
                        },
                        new
                        {
                            IdType = 2,
                            Name = "Chef"
                        },
                        new
                        {
                            IdType = 3,
                            Name = "Chef helper"
                        },
                        new
                        {
                            IdType = 4,
                            Name = "Waiter"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Reservation", b =>
                {
                    b.Property<int>("IdReservation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdReservation"), 1L, 1);

                    b.Property<int>("HowManyPeoples")
                        .HasColumnType("int");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdRestaurant")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReservationGrade")
                        .HasColumnType("int");

                    b.Property<string>("ReservationStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdReservation");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdRestaurant");

                    b.ToTable("Reservation");

                    b.HasData(
                        new
                        {
                            IdReservation = 1,
                            HowManyPeoples = 1,
                            IdClient = 1,
                            IdRestaurant = 1,
                            ReservationDate = new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ReservationStatus = "NEW"
                        },
                        new
                        {
                            IdReservation = 2,
                            HowManyPeoples = 4,
                            IdClient = 3,
                            IdRestaurant = 1,
                            ReservationDate = new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ReservationStatus = "CANCELED"
                        },
                        new
                        {
                            IdReservation = 3,
                            HowManyPeoples = 6,
                            IdClient = 2,
                            IdRestaurant = 1,
                            ReservationDate = new DateTime(2023, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ReservationGrade = 4,
                            ReservationStatus = "RATED"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Restaurant", b =>
                {
                    b.Property<int>("IdRestaurant")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRestaurant"), 1L, 1);

                    b.Property<decimal?>("BonusBudget")
                        .HasColumnType("money");

                    b.Property<int>("IdAddress")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RestaurantStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdRestaurant");

                    b.HasIndex("IdAddress");

                    b.ToTable("Restaurant");

                    b.HasData(
                        new
                        {
                            IdRestaurant = 1,
                            BonusBudget = 1230m,
                            IdAddress = 1,
                            Name = "Pod Lasem",
                            RestaurantStatus = "Working"
                        },
                        new
                        {
                            IdRestaurant = 2,
                            BonusBudget = 400m,
                            IdAddress = 2,
                            Name = "Zapiecek",
                            RestaurantStatus = "Under construction"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.RestaurantDish", b =>
                {
                    b.Property<int>("IdRestaurantDish")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRestaurantDish"), 1L, 1);

                    b.Property<int>("IdDish")
                        .HasColumnType("int");

                    b.Property<int>("IdRestaurant")
                        .HasColumnType("int");

                    b.HasKey("IdRestaurantDish");

                    b.HasIndex("IdDish");

                    b.HasIndex("IdRestaurant");

                    b.ToTable("RestaurantDish");

                    b.HasData(
                        new
                        {
                            IdRestaurantDish = 1,
                            IdDish = 1,
                            IdRestaurant = 1
                        },
                        new
                        {
                            IdRestaurantDish = 2,
                            IdDish = 2,
                            IdRestaurant = 1
                        },
                        new
                        {
                            IdRestaurantDish = 3,
                            IdDish = 3,
                            IdRestaurant = 1
                        },
                        new
                        {
                            IdRestaurantDish = 4,
                            IdDish = 4,
                            IdRestaurant = 1
                        },
                        new
                        {
                            IdRestaurantDish = 5,
                            IdDish = 5,
                            IdRestaurant = 1
                        },
                        new
                        {
                            IdRestaurantDish = 6,
                            IdDish = 6,
                            IdRestaurant = 1
                        },
                        new
                        {
                            IdRestaurantDish = 7,
                            IdDish = 6,
                            IdRestaurant = 2
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.DatabaseModel.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"), 1L, 1);

                    b.Property<DateTime?>("DateBlockedTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("IdClient")
                        .HasColumnType("int");

                    b.Property<int?>("IdEmployee")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdUser");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdEmployee");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Complaint", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Reservation", "Reservation")
                        .WithOne("Complaint")
                        .HasForeignKey("Restaurants_REST_API.Models.Database.Complaint", "IdReservation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Database.Restaurant", "Restaurant")
                        .WithMany("Complaints")
                        .HasForeignKey("IdRestaurant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Employee", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Address", "Address")
                        .WithMany("Employees")
                        .HasForeignKey("IdAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.EmployeeCertificate", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Certificate", "Certificate")
                        .WithMany("EmployeeCertificates")
                        .HasForeignKey("IdCertificate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Database.Employee", "Employee")
                        .WithMany("EmployeeCertificates")
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Certificate");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.EmployeeRestaurant", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Employee", "Employee")
                        .WithMany("EmployeeInRestaurant")
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Database.Restaurant", "Restaurant")
                        .WithMany("RestaurantEmployees")
                        .HasForeignKey("IdRestaurant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Database.EmployeeType", "EmployeeType")
                        .WithMany("EmployeeTypes")
                        .HasForeignKey("IdType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("EmployeeType");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Reservation", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Client", "Clients")
                        .WithMany("Reservations")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Database.Restaurant", "Restaurant")
                        .WithMany("Reservations")
                        .HasForeignKey("IdRestaurant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clients");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Restaurant", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Address", "Address")
                        .WithMany("Restaurants")
                        .HasForeignKey("IdAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.RestaurantDish", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("IdDish")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Database.Restaurant", "Restaurant")
                        .WithMany("RestaurantDishes")
                        .HasForeignKey("IdRestaurant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.DatabaseModel.User", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Database.Client", "Client")
                        .WithMany()
                        .HasForeignKey("IdClient");

                    b.HasOne("Restaurants_REST_API.Models.Database.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("IdEmployee");

                    b.Navigation("Client");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Address", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Restaurants");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Certificate", b =>
                {
                    b.Navigation("EmployeeCertificates");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Client", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Employee", b =>
                {
                    b.Navigation("EmployeeCertificates");

                    b.Navigation("EmployeeInRestaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.EmployeeType", b =>
                {
                    b.Navigation("EmployeeTypes");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Reservation", b =>
                {
                    b.Navigation("Complaint")
                        .IsRequired();
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Database.Restaurant", b =>
                {
                    b.Navigation("Complaints");

                    b.Navigation("Reservations");

                    b.Navigation("RestaurantDishes");

                    b.Navigation("RestaurantEmployees");
                });
#pragma warning restore 612, 618
        }
    }
}
