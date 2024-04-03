using CityCompareProxy.Models;
using CityCompareProxy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CityCompareProxy.Repositories
{
    public class ScbRepository : IScbRepository
    {
        private readonly AppDbContext _appDbContext;

        public ScbRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task StoreCity(City city)
        {
            await _appDbContext.Cities.AddAsync(city);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<City> GetCityAsync(string id)
        {
           return await _appDbContext.Cities.Include(city=> city.HousePrices).ThenInclude(i=>i.Items).FirstOrDefaultAsync(city=>city.Id == id);
        }
    }
}
