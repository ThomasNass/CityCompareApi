using CityCompareProxy.Models;
using CityCompareProxy.Persistence;

namespace CityCompareProxy.Repositories
{
    public class ScbRepository : IScbRepository
    {
        private readonly AppDbContext _appDbContext;

        public ScbRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task StoreResponse(ScbResponse scbResponse)
        {
            await _appDbContext.AddAsync(scbResponse);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task GetResponse(ScbResponse scbResponse)
        {
            await _appDbContext.AddAsync(scbResponse);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
