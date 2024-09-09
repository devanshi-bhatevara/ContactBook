using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService _countryService)
        {
            this._countryService = _countryService;
        }
        [HttpGet("GetAllCountries")]
        public IActionResult GetAllCountries()
        {
            var response = _countryService.GetCountries();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
