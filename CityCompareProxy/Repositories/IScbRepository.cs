using CityCompareProxy.Models;

namespace CityCompareProxy.Repositories
{
    public interface IScbRepository
    {
        void StoreCity(City city);
        City GetCity(string id);
        void UpdateCity(City city);
        public void Add<T>(T entity) where T : DataEntity;
    }
}