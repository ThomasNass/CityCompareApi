
using CityCompareProxy.Models;
using Microsoft.EntityFrameworkCore;

namespace CityCompareProxy.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<HousePrices> HousePrices { get; set; }
        public DbSet<Data> Data { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
