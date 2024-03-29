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
    [Migration("20230320145048_AddedDishAndDishesInRestaurantTables")]
    partial class AddedDishAndDishesInRestaurantTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Restaurants_REST_API.Models.Address", b =>
                {
                    b.Property<int>("IdAddress")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAddress"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NoBuilding")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("NoLocal")
                        .IsRequired()
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
                            City = "Warsaw",
                            NoBuilding = "11",
                            NoLocal = "1",
                            Street = "John Paul II"
                        },
                        new
                        {
                            IdAddress = 2,
                            City = "Warsaw",
                            NoBuilding = "1",
                            NoLocal = "2",
                            Street = "John Paul II"
                        },
                        new
                        {
                            IdAddress = 3,
                            City = "Warsaw",
                            NoBuilding = "78",
                            NoLocal = "32A",
                            Street = "Stawki"
                        },
                        new
                        {
                            IdAddress = 4,
                            City = "Warsaw",
                            NoBuilding = "14",
                            NoLocal = "",
                            Street = "Romualda"
                        },
                        new
                        {
                            IdAddress = 5,
                            City = "Warsaw",
                            NoBuilding = "B2",
                            NoLocal = "21",
                            Street = "Skret"
                        },
                        new
                        {
                            IdAddress = 6,
                            City = "Warsaw",
                            NoBuilding = "A1",
                            NoLocal = "A1",
                            Street = "Koszyk"
                        },
                        new
                        {
                            IdAddress = 7,
                            City = "Warsaw",
                            NoBuilding = "21",
                            NoLocal = "37",
                            Street = "John Paul II"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Certificate", b =>
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

                    b.ToTable("Certificates");

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

            modelBuilder.Entity("Restaurants_REST_API.Models.Client", b =>
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

                    b.ToTable("Clients");

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

            modelBuilder.Entity("Restaurants_REST_API.Models.Dish", b =>
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

                    b.ToTable("Dishes");

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

            modelBuilder.Entity("Restaurants_REST_API.Models.DishInRestaurant", b =>
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

                    b.ToTable("RestaurantDishes");

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

            modelBuilder.Entity("Restaurants_REST_API.Models.Employee", b =>
                {
                    b.Property<int>("IdEmployee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmployee"), 1L, 1);

                    b.Property<DateTime>("HiredDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdAddress")
                        .HasColumnType("int");

                    b.Property<string>("IsHealthBook")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("IsOwner")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PESEL")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdEmployee");

                    b.HasIndex("IdAddress");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            IdEmployee = 1,
                            HiredDate = new DateTime(1999, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 3,
                            IsHealthBook = "Y",
                            IsOwner = "Y",
                            Name = "Michal",
                            PESEL = "11122233344",
                            Salary = 3421m,
                            Surname = "Nowak"
                        },
                        new
                        {
                            IdEmployee = 2,
                            HiredDate = new DateTime(2002, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 4,
                            IsHealthBook = "Y",
                            IsOwner = "N",
                            Name = "Kacper",
                            PESEL = "22233344455",
                            Salary = 2217m,
                            Surname = "Kawka"
                        },
                        new
                        {
                            IdEmployee = 3,
                            HiredDate = new DateTime(2004, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 5,
                            IsHealthBook = "Y",
                            IsOwner = "N",
                            Name = "Janusz",
                            PESEL = "33344455566",
                            Salary = 2213m,
                            Surname = "Jogurt"
                        },
                        new
                        {
                            IdEmployee = 4,
                            HiredDate = new DateTime(2005, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 6,
                            IsHealthBook = "Y",
                            IsOwner = "N",
                            Name = "Bozena",
                            PESEL = "44455566677",
                            Salary = 2613m,
                            Surname = "Lawenda"
                        },
                        new
                        {
                            IdEmployee = 5,
                            HiredDate = new DateTime(2010, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdAddress = 7,
                            IsHealthBook = "Y",
                            IsOwner = "N",
                            Name = "Joanna",
                            PESEL = "55566677788",
                            Salary = 2913m,
                            Surname = "Skrzynka"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.EmployeeCertificates", b =>
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

                    b.ToTable("EmployeeCertificates");

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

            modelBuilder.Entity("Restaurants_REST_API.Models.EmployeesInRestaurant", b =>
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

                    b.ToTable("EmployeesInRestaurants");

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

            modelBuilder.Entity("Restaurants_REST_API.Models.EmployeeType", b =>
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

                    b.ToTable("EmployeeTypes");

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

            modelBuilder.Entity("Restaurants_REST_API.Models.Reservation", b =>
                {
                    b.Property<int>("IdReservation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdReservation"), 1L, 1);

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdRestauration")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TableNumber")
                        .HasColumnType("int");

                    b.HasKey("IdReservation");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdRestauration");

                    b.ToTable("Reservations");

                    b.HasData(
                        new
                        {
                            IdReservation = 1,
                            IdClient = 1,
                            IdRestauration = 1,
                            ReservationDate = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TableNumber = 1
                        },
                        new
                        {
                            IdReservation = 2,
                            IdClient = 3,
                            IdRestauration = 1,
                            ReservationDate = new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TableNumber = 2
                        },
                        new
                        {
                            IdReservation = 3,
                            IdClient = 2,
                            IdRestauration = 1,
                            ReservationDate = new DateTime(2023, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TableNumber = 1
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Restaurant", b =>
                {
                    b.Property<int>("IdRestaurant")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRestaurant"), 1L, 1);

                    b.Property<int>("IdAddress")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdRestaurant");

                    b.HasIndex("IdAddress");

                    b.ToTable("Restaurants");

                    b.HasData(
                        new
                        {
                            IdRestaurant = 1,
                            IdAddress = 1,
                            Name = "Pod Lasem"
                        },
                        new
                        {
                            IdRestaurant = 2,
                            IdAddress = 2,
                            Name = "Zapiecek"
                        });
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.DishInRestaurant", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Dish", "Dish")
                        .WithMany("DishInRestaurants")
                        .HasForeignKey("IdDish")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Restaurant", "Restaurant")
                        .WithMany("RestaurantDishes")
                        .HasForeignKey("IdRestaurant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Employee", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Address", "Address")
                        .WithMany("Employees")
                        .HasForeignKey("IdAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.EmployeeCertificates", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Certificate", "Certificate")
                        .WithMany("EmployeeCertificates")
                        .HasForeignKey("IdCertificate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Employee", "Employee")
                        .WithMany("EmployeeCertificates")
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Certificate");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.EmployeesInRestaurant", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Employee", "Employee")
                        .WithMany("EmployeeInRestaurant")
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Restaurant", "Restaurant")
                        .WithMany("RestaurantEmployees")
                        .HasForeignKey("IdRestaurant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.EmployeeType", "EmployeeType")
                        .WithMany("EmployeeTypes")
                        .HasForeignKey("IdType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("EmployeeType");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Reservation", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Client", "Clients")
                        .WithMany("Reservations")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Restaurants_REST_API.Models.Restaurant", "Restaurant")
                        .WithMany("Reservations")
                        .HasForeignKey("IdRestauration")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clients");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Restaurant", b =>
                {
                    b.HasOne("Restaurants_REST_API.Models.Address", "Address")
                        .WithMany("Restaurants")
                        .HasForeignKey("IdAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Address", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Restaurants");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Certificate", b =>
                {
                    b.Navigation("EmployeeCertificates");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Client", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Dish", b =>
                {
                    b.Navigation("DishInRestaurants");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Employee", b =>
                {
                    b.Navigation("EmployeeCertificates");

                    b.Navigation("EmployeeInRestaurant");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.EmployeeType", b =>
                {
                    b.Navigation("EmployeeTypes");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Restaurant", b =>
                {
                    b.Navigation("Reservations");

                    b.Navigation("RestaurantDishes");

                    b.Navigation("RestaurantEmployees");
                });
#pragma warning restore 612, 618
        }
    }
}
