using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface IStateRepository
    {
        IEnumerable<State> GetStatesByCountryId(int countryId);
        IEnumerable<State> GetAll();
    }
}
