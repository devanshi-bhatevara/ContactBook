using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface IAuthRepository
    {
        bool RegisterUser(User user);

        User? ValidateUser(string username);

        bool UserExist(string loginId, string email);

        bool UserExist(int userId, string loginId, string email);

        bool UpdateUser(User user);

        User? GetUser(string id);
    }
}
