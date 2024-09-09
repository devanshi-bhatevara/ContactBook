using ContactBookApi.Data.Contract;
using ContactBookApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ContactBookApi.Data.Implementation
{
    public class StateRepository : IStateRepository
    {
        private readonly IAppDbContext _appDbContext;

        public StateRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<State> GetStatesByCountryId(int countryId)
        {
            return _appDbContext.states
                .Where(s => s.CountryId == countryId)
                .ToList();
        }

        public IEnumerable<State> GetAll()
        {
            List<State> categories = _appDbContext.states.Include(p => p.Country).ToList();
            return categories;

        }
    }
}
