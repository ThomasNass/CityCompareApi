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

        public void StoreCity(City city)
        {
            _appDbContext.Cities.Add(city);
            _appDbContext.SaveChanges();
        }
        public void UpdateCity(City city)
        {
            City cityToUpdate = _appDbContext.Cities.FirstOrDefault(c => c.LauCode == city.LauCode);
            if (cityToUpdate != null)
            {
               // cityToUpdate = city;
                _appDbContext.SaveChanges();
            }
        }

        public void Add<T>(T entity) where T : DataEntity
        {
            _appDbContext.Set<T>().Add(entity);
            _appDbContext.SaveChanges();
        }

        public City GetCity(string id)
        {
           return  _appDbContext.Cities
                .Include(city=> city.HousePrices)
                .ThenInclude(i=>i.Items)
                .Include(city => city.PopulationGrowth)
                .ThenInclude(i => i.Items)
                .Include(city => city.Income)
                .ThenInclude(i => i.Items)
                .Include(city => city.PopulationGenderData)
                .ThenInclude(i => i.Items)
                .Include(city => city.ElectionData)
                .ThenInclude(i => i.Items)
                .Include(city => city.MunicipalityElectionData)
                .ThenInclude(i => i.Items)
                .FirstOrDefault(city => city.LauCode == id);
        }
    }
}
