
using CityCompareProxy.Models;
using Microsoft.EntityFrameworkCore;

namespace CityCompareProxy.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<ScbResponse> ScbResponse { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
