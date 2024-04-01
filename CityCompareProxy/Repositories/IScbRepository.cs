using CityCompareProxy.Models;

namespace CityCompareProxy.Repositories
{
    public interface IScbRepository
    {
        Task StoreResponse(ScbResponse scbResponse);
    }
}