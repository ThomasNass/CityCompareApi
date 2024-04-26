using CityCompareProxy.Models;

namespace CityCompareProxy.Services
{
    public interface IScbService
    {
        Task<City?> GetElectionData(string lauCode);
        Task<City?> GetGrowthData(string lauCode);
        Task<City?> GetHousePrice(string lauCode);
        Task<City?> GetIncomeData(string lauCode);
        Task<City?> GetMunicipalityElectionData(string lauCode);
        Task<City?> GetPopulationGenderData(string lauCode);
        Task<City> GetCityAsync(string lauCode);
        Task PopulateCityWithData(string lauCode);
    }
}