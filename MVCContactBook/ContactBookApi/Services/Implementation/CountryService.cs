using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Services.Contract;

namespace ContactBookApi.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;

        public CountryService(ICountryRepository _repository)
        {
            this._repository = _repository;
        }

        public ServiceResponse<IEnumerable<CountryDto>> GetCountries()
        {
            var response = new ServiceResponse<IEnumerable<CountryDto>>();
            var categories = _repository.GetAll();
            if (categories != null && categories.Any())
            {

                List<CountryDto> countryDtos = new List<CountryDto>();

                foreach (var country in categories)
                {
                    countryDtos.Add(new CountryDto() { CountryId = country.CountryId, CountryName = country.CountryName });
                }
                response.Data = countryDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }
    }
}
