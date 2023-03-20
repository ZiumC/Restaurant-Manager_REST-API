﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurants_REST_API.DbContexts;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Restaurants_REST_API.Models.Client", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Restaurants_REST_API.Models.Restaurant", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
