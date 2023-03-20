using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.Models;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DbContexts
{
    public class MainDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public MainDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MainDbContext(DbContextOptions opt) : base(opt)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:Default"]);
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

        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
