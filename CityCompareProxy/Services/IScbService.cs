using CityCompareProxy.Models;

namespace CityCompareProxy.Services
{
    public interface IScbService
    {
        Task<ScbResponse?> GetElectionData(string cityId);
        Task<ScbResponse?> GetGrowthData(string cityId);
        Task<City?> GetHousePrice(string cityId);
        Task<ScbResponse?> GetIncomeData(string cityId);
        Task<ScbResponse?> GetMunicipalityElectionData(string cityId);
        Task<ScbResponse?> GetPopulationData(string cityId);
        Task<City> GetCityAsync(string cityId);
    }
}