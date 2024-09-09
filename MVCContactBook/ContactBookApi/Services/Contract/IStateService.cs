using ContactBookApi.Dtos;
using ContactBookApi.Models;

namespace ContactBookApi.Services.Contract
{
    public interface IStateService
    {
        ServiceResponse<IEnumerable<StateDto>> GetStatesByCountryId(int id);

        ServiceResponse<IEnumerable<StateDto>> GetStates();
    }
}
