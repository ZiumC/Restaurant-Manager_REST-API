using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.Models;
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
                a.Property(e => e.NoBuilding).IsRequired().HasMaxLength(5);
                a.Property(e => e.NoLocal).HasMaxLength(5);

                a.HasData(new Address { IdAddress = 1, City = "Warsaw", Street = "John Paul II", NoBuilding = "11", NoLocal = "1" });
                a.HasData(new Address { IdAddress = 2, City = "Warsaw", Street = "John Paul II", NoBuilding = "1", NoLocal = "2" });
                a.HasData(new Address { IdAddress = 3, City = "Warsaw", Street = "Stawki", NoBuilding = "78", NoLocal = "32A" });
                a.HasData(new Address { IdAddress = 4, City = "Warsaw", Street = "Romualda", NoBuilding = "14", NoLocal = "" });
                a.HasData(new Address { IdAddress = 5, City = "Warsaw", Street = "Skret", NoBuilding = "B2", NoLocal = "21" });
                a.HasData(new Address { IdAddress = 6, City = "Warsaw", Street = "Koszyk", NoBuilding = "A1", NoLocal = "A1" });
                a.HasData(new Address { IdAddress = 7, City = "Warsaw", Street = "John Paul II", NoBuilding = "21", NoLocal = "37" });
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

                r.HasData(new Restaurant { IdRestaurant = 1, Name = "Pod Lasem", IdAddress = 1 });
                r.HasData(new Restaurant { IdRestaurant = 2, Name = "Zapiecek", IdAddress = 2 });
            });

            modelBuilder.Entity<Reservation>(r =>
            {
                r.HasKey(e => e.IdReservation);
                r.Property(e => e.ReservationDate).IsRequired();
                r.Property(e => e.TableNumber).IsRequired();

                r.HasData(new Reservation { IdReservation = 1, ReservationDate = DateTime.Parse("2023-01-01"), TableNumber = 1, IdClient = 1, IdRestauration = 1 });
                r.HasData(new Reservation { IdReservation = 2, ReservationDate = DateTime.Parse("2023-11-01"), TableNumber = 2, IdClient = 3, IdRestauration = 1 });
                r.HasData(new Reservation { IdReservation = 3, ReservationDate = DateTime.Parse("2023-02-09"), TableNumber = 1, IdClient = 2, IdRestauration = 1 });
            });



            modelBuilder.Entity<Employee>(e =>
            {
                e.HasKey(e => e.IdEmployee);
                e.Property(e => e.Salary).IsRequired().HasColumnType("money");
                e.Property(e => e.HiredDate).IsRequired();
                e.Property(e => e.IsHealthBook).IsRequired().HasMaxLength(1);
                e.Property(e => e.Name).IsRequired().HasMaxLength(50);
                e.Property(e => e.Surname).IsRequired().HasMaxLength(50);
                e.Property(e => e.PESEL).IsRequired().HasMaxLength(11);
                e.Property(e => e.IsOwner).IsRequired().HasMaxLength(1);

                e.HasData(new Employee
                {
                    IdEmployee = 1,
                    Salary = 3421,
                    HiredDate = DateTime.Parse("1999-01-04"),
                    IsHealthBook = "Y",
                    Name = "Michal",
                    Surname = "Nowak",
                    PESEL = "11122233344",
                    IsOwner = "Y",
                    IdAddress = 3
                });

                e.HasData(new Employee
                {
                    IdEmployee = 2,
                    Salary = 2217,
                    HiredDate = DateTime.Parse("2002-03-04"),
                    IsHealthBook = "Y",
                    Name = "Kacper",
                    Surname = "Kawka",
                    PESEL = "22233344455",
                    IsOwner = "N",
                    IdAddress = 4
                });

                e.HasData(new Employee
                {
                    IdEmployee = 3,
                    Salary = 2213,
                    HiredDate = DateTime.Parse("2004-02-11"),
                    IsHealthBook = "Y",
                    Name = "Janusz",
                    Surname = "Jogurt",
                    PESEL = "33344455566",
                    IsOwner = "N",
                    IdAddress = 5
                });

                e.HasData(new Employee
                {
                    IdEmployee = 4,
                    Salary = 2613,
                    HiredDate = DateTime.Parse("2005-05-12"),
                    IsHealthBook = "Y",
                    Name = "Bozena",
                    Surname = "Lawenda",
                    PESEL = "44455566677",
                    IsOwner = "N",
                    IdAddress = 6
                });

                e.HasData(new Employee
                {
                    IdEmployee = 5,
                    Salary = 2913,
                    HiredDate = DateTime.Parse("2010-11-02"),
                    IsHealthBook = "Y",
                    Name = "Joanna",
                    Surname = "Skrzynka",
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

            modelBuilder.Entity<EmployeeCertificates>(ec =>
            {
                ec.HasKey(e => e.IdEmployeeCertificate);
                ec.Property(e => e.ExpirationDate).IsRequired();

                ec.HasData(new EmployeeCertificates { IdEmployeeCertificate = 1, IdCertificate = 1, IdEmployee = 1, ExpirationDate = DateTime.Parse("2023-11-11") });
                ec.HasData(new EmployeeCertificates { IdEmployeeCertificate = 2, IdCertificate = 1, IdEmployee = 2, ExpirationDate = DateTime.Parse("2023-11-11") });
                ec.HasData(new EmployeeCertificates { IdEmployeeCertificate = 3, IdCertificate = 2, IdEmployee = 1, ExpirationDate = DateTime.Parse("2023-10-09") });
                ec.HasData(new EmployeeCertificates { IdEmployeeCertificate = 4, IdCertificate = 2, IdEmployee = 4, ExpirationDate = DateTime.Parse("2023-10-09") });
                ec.HasData(new EmployeeCertificates { IdEmployeeCertificate = 5, IdCertificate = 2, IdEmployee = 3, ExpirationDate = DateTime.Parse("2023-10-09") });
                ec.HasData(new EmployeeCertificates { IdEmployeeCertificate = 6, IdCertificate = 3, IdEmployee = 1, ExpirationDate = DateTime.Parse("2023-11-12") });
                ec.HasData(new EmployeeCertificates { IdEmployeeCertificate = 7, IdCertificate = 3, IdEmployee = 5, ExpirationDate = DateTime.Parse("2023-11-12") });
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

            modelBuilder.Entity<EmployeesInRestaurant>(eir =>
            {
                eir.HasKey(e => e.IdRestaurantWorker);

                eir.HasData(new EmployeesInRestaurant { IdRestaurantWorker = 1, IdType = 1, IdEmployee = 1, IdRestaurant = 1 });
                eir.HasData(new EmployeesInRestaurant { IdRestaurantWorker = 2, IdType = 2, IdEmployee = 2, IdRestaurant = 1 });
                eir.HasData(new EmployeesInRestaurant { IdRestaurantWorker = 3, IdType = 3, IdEmployee = 3, IdRestaurant = 1 });
                eir.HasData(new EmployeesInRestaurant { IdRestaurantWorker = 4, IdType = 3, IdEmployee = 4, IdRestaurant = 1 });
                eir.HasData(new EmployeesInRestaurant { IdRestaurantWorker = 5, IdType = 4, IdEmployee = 5, IdRestaurant = 1 });
                eir.HasData(new EmployeesInRestaurant { IdRestaurantWorker = 6, IdType = 1, IdEmployee = 1, IdRestaurant = 2 });
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


            modelBuilder.Entity<DishInRestaurant>(d =>
            {
                d.HasKey(e => e.IdRestaurantDish);

                d.HasData(new DishInRestaurant { IdRestaurantDish = 1, IdDish = 1, IdRestaurant = 1 });
                d.HasData(new DishInRestaurant { IdRestaurantDish = 2, IdDish = 2, IdRestaurant = 1 });
                d.HasData(new DishInRestaurant { IdRestaurantDish = 3, IdDish = 3, IdRestaurant = 1 });
                d.HasData(new DishInRestaurant { IdRestaurantDish = 4, IdDish = 4, IdRestaurant = 1 });
                d.HasData(new DishInRestaurant { IdRestaurantDish = 5, IdDish = 5, IdRestaurant = 1 });
                d.HasData(new DishInRestaurant { IdRestaurantDish = 6, IdDish = 6, IdRestaurant = 1 });
                d.HasData(new DishInRestaurant { IdRestaurantDish = 7, IdDish = 6, IdRestaurant = 2 });
            });

        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<EmployeeCertificates> EmployeeCertificates { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<EmployeesInRestaurant> EmployeesInRestaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishInRestaurant> RestaurantDishes { get; set; }
    }
}
