using CityCompareProxy.Models;

namespace CityCompareProxy.Repositories
{
    public interface IScbRepository
    {
        Task StoreCity(City city);
        Task<City> GetCityAsync(string id);
    }
}