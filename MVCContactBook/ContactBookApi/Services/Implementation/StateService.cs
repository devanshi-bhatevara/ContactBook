using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;

namespace ContactBookApi.Services.Implementation
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }
        public ServiceResponse<IEnumerable<StateDto>> GetStatesByCountryId(int id)
        {
            var response = new ServiceResponse<IEnumerable<StateDto>>();

            var states = _stateRepository.GetStatesByCountryId(id);
            if (states != null && states.Any())
            {
                // Map State entities to StateDto objects
                List<StateDto> stateDtos = new List<StateDto>();
                foreach (var state in states)
                {
                    stateDtos.Add(new StateDto() { StateId = state.StateId, StateName = state.StateName, CountryId = state.CountryId });
                }

                response.Data = stateDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No states found for the specified country.";
            }

            return response;
        }

        public ServiceResponse<IEnumerable<StateDto>> GetStates()
        {
            var response = new ServiceResponse<IEnumerable<StateDto>>();
            var categories = _stateRepository.GetAll();
            if (categories != null && categories.Any())
            {

                List<StateDto> stateDtos = new List<StateDto>();

                foreach (var state in categories)
                {
                    stateDtos.Add(new StateDto()
                    {
                        StateId = state.StateId,
                        StateName = state.StateName,
                        CountryId = state.CountryId,
                        Country = new Country()
                        {
                            CountryId = state.Country.CountryId,
                            CountryName = state.Country.CountryName
                        },
                    });
                }
                response.Data = stateDtos;
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
