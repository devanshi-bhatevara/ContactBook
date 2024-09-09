using ContactBookApi.Data.Contract;
using ContactBookApi.Models;

namespace ContactBookApi.Data.Implementation
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IAppDbContext _context;

        public CountryRepository(IAppDbContext _context)
        {
            this._context = _context;
        }
        public IEnumerable<Country> GetAll()
        {
            List<Country> countries = _context.countries.ToList();
            return countries;

        }

    }
}

