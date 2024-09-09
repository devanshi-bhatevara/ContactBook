using ContactBookApi.Dtos;

namespace ContactBookApi.Services.Contract
{
    public interface ICountryService
    {
        ServiceResponse<IEnumerable<CountryDto>> GetCountries();
    }
}
