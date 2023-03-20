using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.Models;

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

        public DbSet<Address> Address { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }


    }
}
