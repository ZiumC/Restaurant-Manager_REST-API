﻿using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Models.DatabaseModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DbContexts
{
    public class MainDbContext : DbContext
    {
        public MainDbContext()
        {
        }

        public MainDbContext(DbContextOptions opt) : base(opt)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:Default"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>(a =>
            {
                a.HasKey(e => e.IdAddress);
                a.Property(e => e.City).IsRequired().HasMaxLength(50);
                a.Property(e => e.Street).IsRequired().HasMaxLength(50);
                a.Property(e => e.BuildingNumber).IsRequired().HasMaxLength(5);
                a.Property(e => e.LocalNumber).HasMaxLength(5);

                a.HasData(new Address { IdAddress = 1, City = "Warsaw", Street = "John Paul II", BuildingNumber = "11", LocalNumber = "1" });
                a.HasData(new Address { IdAddress = 2, City = "Warsaw", Street = "John Paul II", BuildingNumber = "1", LocalNumber = "2" });
                a.HasData(new Address { IdAddress = 3, City = "Warsaw", Street = "Stawki", BuildingNumber = "78", LocalNumber = "32A" });
                a.HasData(new Address { IdAddress = 4, City = "Warsaw", Street = "Romualda", BuildingNumber = "14", LocalNumber = null });
                a.HasData(new Address { IdAddress = 5, City = "Warsaw", Street = "Skret", BuildingNumber = "B2", LocalNumber = "21" });
                a.HasData(new Address { IdAddress = 6, City = "Warsaw", Street = "Koszyk", BuildingNumber = "A1", LocalNumber = null });
                a.HasData(new Address { IdAddress = 7, City = "Warsaw", Street = "John Paul II", BuildingNumber = "21", LocalNumber = "37" });
            });


            modelBuilder.Entity<Client>(c =>
            {
                c.HasKey(e => e.IdClient);
                c.Property(e => e.Name).IsRequired().HasMaxLength(50);
                c.Property(e => e.IsBusinessman).IsRequired().HasMaxLength(1);

                c.HasData(new Client { IdClient = 1, Name = "Jan", IsBusinessman = "N" });
                c.HasData(new Client { IdClient = 2, Name = "Michal", IsBusinessman = "N" });
                c.HasData(new Client { IdClient = 3, Name = "Joanna", IsBusinessman = "Y" });
            });

            modelBuilder.Entity<Restaurant>(r =>
            {
                r.HasKey(e => e.IdRestaurant);
                r.Property(e => e.Name).IsRequired().HasMaxLength(100);
                r.Property(e => e.BonusBudget).HasColumnType("money");
                r.Property(e => e.RestaurantStatus).IsRequired().HasMaxLength(50);

                r.HasData(new Restaurant { IdRestaurant = 1, Name = "Pod Lasem", RestaurantStatus = "Working", BonusBudget = 1230, IdAddress = 1 });
                r.HasData(new Restaurant { IdRestaurant = 2, Name = "Zapiecek", RestaurantStatus = "Under construction", BonusBudget = 400, IdAddress = 2 });
            });

            modelBuilder.Entity<Reservation>(r =>
            {
                r.HasKey(e => e.IdReservation);
                r.Property(e => e.ReservationDate).IsRequired();
                r.Property(e => e.HowManyPeoples).IsRequired();
                r.Property(e => e.ReservationStatus).IsRequired().HasMaxLength(50);

                r.HasData(new Reservation
                {
                    IdReservation = 1,
                    ReservationDate = DateTime.Parse("2023-12-12"),
                    HowManyPeoples = 1,
                    ReservationStatus = "NEW",
                    ReservationGrade = null,
                    IdClient = 1,
                    IdRestaurant = 1
                });
                r.HasData(new Reservation
                {
                    IdReservation = 2,
                    ReservationDate = DateTime.Parse("2023-11-01"),
                    HowManyPeoples = 4,
                    ReservationStatus = "CANCELED",
                    ReservationGrade = null,
                    IdClient = 3,
                    IdRestaurant = 1
                });
                r.HasData(new Reservation
                {
                    IdReservation = 3,
                    ReservationDate = DateTime.Parse("2023-02-09"),
                    HowManyPeoples = 6,
                    ReservationStatus = "RATED",
                    ReservationGrade = 4,
                    IdClient = 2,
                    IdRestaurant = 1
                });
            });

            modelBuilder.Entity<Complaint>(c =>
            {
                c.HasKey(e => e.IdComplaint);
                c.Property(e => e.ComplainDate).IsRequired();
                c.Property(e => e.ComplaintStatus).IsRequired().HasMaxLength(50);
                c.Property(e => e.ComplaintMessage).IsRequired().HasMaxLength(350);

                c.HasData(new Complaint
                {
                    IdComplaint = 1,
                    ComplainDate = DateTime.Parse("2023-01-01"),
                    ComplaintStatus = "REJECTED",
                    ComplaintMessage = "Zupa była za słona, obsługa była niemiła",
                    IdReservation = 1,
                    IdRestaurant = 1
                });

                c.HasData(new Complaint
                {
                    IdComplaint = 2,
                    ComplainDate = DateTime.Parse("2023-11-08"),
                    ComplaintStatus = "NEW",
                    ComplaintMessage = "Na kotlecie była mucha",
                    IdReservation = 3,
                    IdRestaurant = 1
                });
            }
            );

            modelBuilder.Entity<Employee>(e =>
            {
                e.HasKey(e => e.IdEmployee);
                e.Property(e => e.Salary).IsRequired().HasColumnType("money");
                e.Property(e => e.BonusSalary).IsRequired().HasColumnType("money");
                e.Property(e => e.HiredDate).IsRequired();
                e.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                e.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                e.Property(e => e.PESEL).IsRequired().HasMaxLength(11);
                e.Property(e => e.IsOwner).IsRequired().HasMaxLength(1);

                e.HasData(new Employee
                {
                    IdEmployee = 1,
                    Salary = 3421,
                    BonusSalary = 150,
                    HiredDate = DateTime.Parse("1999-01-04"),
                    FirstPromotionChefDate = DateTime.Parse("2000-01-04"),
                    FirstName = "Michal",
                    LastName = "Nowak",
                    PESEL = "11122233344",
                    IsOwner = "Y",
                    IdAddress = 3
                });

                e.HasData(new Employee
                {
                    IdEmployee = 2,
                    Salary = 2217,
                    BonusSalary = 150,
                    HiredDate = DateTime.Parse("2002-03-04"),
                    FirstPromotionChefDate = null,
                    FirstName = "Kacper",
                    LastName = "Kawka",
                    PESEL = "22233344455",
                    IsOwner = "N",
                    IdAddress = 4
                });

                e.HasData(new Employee
                {
                    IdEmployee = 3,
                    Salary = 2213,
                    BonusSalary = 150,
                    HiredDate = DateTime.Parse("2004-02-11"),
                    FirstPromotionChefDate = null,
                    FirstName = "Janusz",
                    LastName = "Jogurt",
                    PESEL = "33344455566",
                    IsOwner = "N",
                    IdAddress = 5
                });

                e.HasData(new Employee
                {
                    IdEmployee = 4,
                    Salary = 2613,
                    BonusSalary = 150,
                    HiredDate = DateTime.Parse("2005-05-12"),
                    FirstPromotionChefDate = null,
                    FirstName = "Bozena",
                    LastName = "Lawenda",
                    PESEL = "44455566677",
                    IsOwner = "N",
                    IdAddress = 6
                });

                e.HasData(new Employee
                {
                    IdEmployee = 5,
                    Salary = 2913,
                    BonusSalary = 150,
                    HiredDate = DateTime.Parse("2010-11-02"),
                    FirstPromotionChefDate = null,
                    FirstName = "Joanna",
                    LastName = "Skrzynka",
                    PESEL = "55566677788",
                    IsOwner = "N",
                    IdAddress = 7
                });
            });

            modelBuilder.Entity<Certificate>(c =>
            {
                c.HasKey(e => e.IdCertificate);
                c.Property(e => e.Name).IsRequired().HasMaxLength(125);

                c.HasData(new Certificate { IdCertificate = 1, Name = "Italian Cuisine" });
                c.HasData(new Certificate { IdCertificate = 2, Name = "Germany Cuisine" });
                c.HasData(new Certificate { IdCertificate = 3, Name = "Polish Cuisine" });
            });

            modelBuilder.Entity<EmployeeCertificate>(ec =>
            {
                ec.HasKey(e => e.IdEmployeeCertificate);
                ec.Property(e => e.ExpirationDate).IsRequired();

                ec.HasData(new EmployeeCertificate { IdEmployeeCertificate = 1, IdCertificate = 1, IdEmployee = 1, ExpirationDate = DateTime.Parse("2023-11-11") });
                ec.HasData(new EmployeeCertificate { IdEmployeeCertificate = 2, IdCertificate = 1, IdEmployee = 2, ExpirationDate = DateTime.Parse("2023-11-11") });
                ec.HasData(new EmployeeCertificate { IdEmployeeCertificate = 3, IdCertificate = 2, IdEmployee = 1, ExpirationDate = DateTime.Parse("2023-10-09") });
                ec.HasData(new EmployeeCertificate { IdEmployeeCertificate = 4, IdCertificate = 2, IdEmployee = 4, ExpirationDate = DateTime.Parse("2023-10-09") });
                ec.HasData(new EmployeeCertificate { IdEmployeeCertificate = 5, IdCertificate = 2, IdEmployee = 3, ExpirationDate = DateTime.Parse("2023-10-09") });
                ec.HasData(new EmployeeCertificate { IdEmployeeCertificate = 6, IdCertificate = 3, IdEmployee = 1, ExpirationDate = DateTime.Parse("2023-11-12") });
                ec.HasData(new EmployeeCertificate { IdEmployeeCertificate = 7, IdCertificate = 3, IdEmployee = 5, ExpirationDate = DateTime.Parse("2023-11-12") });
            });

            modelBuilder.Entity<EmployeeType>(et =>
            {
                et.HasKey(e => e.IdType);
                et.Property(e => e.Name).IsRequired().HasMaxLength(50);

                et.HasData(new EmployeeType { IdType = 1, Name = "Owner" });
                et.HasData(new EmployeeType { IdType = 2, Name = "Chef" });
                et.HasData(new EmployeeType { IdType = 3, Name = "Chef helper" });
                et.HasData(new EmployeeType { IdType = 4, Name = "Waiter" });
            });

            modelBuilder.Entity<EmployeeRestaurant>(eir =>
            {
                eir.HasKey(e => e.IdRestaurantWorker);

                eir.HasData(new EmployeeRestaurant { IdRestaurantWorker = 1, IdType = 1, IdEmployee = 1, IdRestaurant = 1 });
                eir.HasData(new EmployeeRestaurant { IdRestaurantWorker = 2, IdType = 2, IdEmployee = 2, IdRestaurant = 1 });
                eir.HasData(new EmployeeRestaurant { IdRestaurantWorker = 3, IdType = 3, IdEmployee = 3, IdRestaurant = 1 });
                eir.HasData(new EmployeeRestaurant { IdRestaurantWorker = 4, IdType = 3, IdEmployee = 4, IdRestaurant = 1 });
                eir.HasData(new EmployeeRestaurant { IdRestaurantWorker = 5, IdType = 4, IdEmployee = 5, IdRestaurant = 1 });
                eir.HasData(new EmployeeRestaurant { IdRestaurantWorker = 6, IdType = 1, IdEmployee = 1, IdRestaurant = 2 });
            });


            modelBuilder.Entity<Dish>(d =>
            {
                d.HasKey(e => e.IdDish);
                d.Property(e => e.Name).IsRequired().HasMaxLength(100);
                d.Property(e => e.Price).IsRequired().HasColumnType("money");

                d.HasData(new Dish { IdDish = 1, Name = "Pizza", Price = new Decimal(19.99) });
                d.HasData(new Dish { IdDish = 2, Name = "Calzone", Price = new Decimal(11.99) });
                d.HasData(new Dish { IdDish = 3, Name = "Pierogi", Price = new Decimal(9.99) });
                d.HasData(new Dish { IdDish = 4, Name = "Brat Wurst", Price = new Decimal(7.99) });
                d.HasData(new Dish { IdDish = 5, Name = "Cheese Bread", Price = new Decimal(19.99) });
                d.HasData(new Dish { IdDish = 6, Name = "Carbonara", Price = new Decimal(12.99) });
            });


            modelBuilder.Entity<RestaurantDish>(d =>
            {
                d.HasKey(e => e.IdRestaurantDish);

                d.HasData(new RestaurantDish { IdRestaurantDish = 1, IdDish = 1, IdRestaurant = 1 });
                d.HasData(new RestaurantDish { IdRestaurantDish = 2, IdDish = 2, IdRestaurant = 1 });
                d.HasData(new RestaurantDish { IdRestaurantDish = 3, IdDish = 3, IdRestaurant = 1 });
                d.HasData(new RestaurantDish { IdRestaurantDish = 4, IdDish = 4, IdRestaurant = 1 });
                d.HasData(new RestaurantDish { IdRestaurantDish = 5, IdDish = 5, IdRestaurant = 1 });
                d.HasData(new RestaurantDish { IdRestaurantDish = 6, IdDish = 6, IdRestaurant = 1 });
                d.HasData(new RestaurantDish { IdRestaurantDish = 7, IdDish = 6, IdRestaurant = 2 });
            });

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(e => e.IdUser);
                u.Property(e => e.Login).IsRequired().HasMaxLength(50);
                u.Property(e => e.Email).IsRequired().HasMaxLength(50);
                u.Property(e => e.Password).IsRequired().HasMaxLength(75);
                u.Property(e => e.PasswordSalt).IsRequired().HasMaxLength(10);
                //1k is here for further expansions
                u.Property(e => e.RefreshToken).HasMaxLength(1000);
                u.Property(e => e.UserRole).IsRequired().HasMaxLength(50);
            });

        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Complaint> Complaint { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<EmployeeCertificate> EmployeeCertificate { get; set; }
        public DbSet<EmployeeType> EmployeeType { get; set; }
        public DbSet<EmployeeRestaurant> EmployeeRestaurant { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<RestaurantDish> RestaurantDish { get; set; }
        public DbSet<User> User { get; set; }
    }
}
