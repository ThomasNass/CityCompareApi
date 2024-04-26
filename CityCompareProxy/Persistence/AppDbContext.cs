
using CityCompareProxy.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CityCompareProxy.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Data> Data { get; set; }
        public DbSet<HousePrices> HousePrices { get; set; }
        public DbSet<PopulationGrowth> PopulationGrowth{ get; set; }
        public DbSet<PopulationGenderData> PopulationGenderData { get; set; }
        public DbSet<Income> Incomes{ get; set; }
        public DbSet<ElectionData> ElectionData { get; set; }
        public DbSet<MunicipalityElectionData> MunicipalityElectionData { get; set; }

        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var jsonData = File.ReadAllText("cities.json");
            var data = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);

            var cityNames = data["cities"];
            var cityIds = data["id"];

            for( var i = 0; i < cityNames.Count; i++)
            {
                modelBuilder.Entity<City>().HasData(

                new City
                {
                    Id = Guid.NewGuid(),
                    LauCode = cityIds[i],
                    Name = cityNames[i]
                }
                );
            }
            
        }
    }
}
