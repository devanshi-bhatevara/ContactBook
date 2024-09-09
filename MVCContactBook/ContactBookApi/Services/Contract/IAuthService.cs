using ContactBookApi.Dtos;
using ContactBookApi.Models;

namespace ContactBookApi.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<string> RegisterUserService(RegisterDto register);
        ServiceResponse<string> LoginUserService(LoginDto login);

        ServiceResponse<string> ForgetPasswordService(ForgetDto forgetDto);

        ServiceResponse<UserDto> GetUser(string userId);

        ServiceResponse<string> ModifyUser(User user, int userId, string email);


    }
}
