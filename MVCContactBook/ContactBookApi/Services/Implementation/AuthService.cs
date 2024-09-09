using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ContactBookApi.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordService _passwordService;

        public AuthService(IAuthRepository authRepository, IPasswordService _passwordService)
        {
            _authRepository = authRepository;
            this._passwordService = _passwordService;

        }

        public ServiceResponse<string> RegisterUserService(RegisterDto register)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                else if (_authRepository.UserExist(register.LoginId, register.Email))
                {
                    response.Success = false;
                    response.Message = "User already exists";
                    return response;

                }
                else
                {
                    User user = new User()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        LoginId = register.LoginId,
                        ContactNumber = register.ContactNumber,
                        FileName = register.FileName,
                        ImageByte = register.ImageByte

                    };

                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    response.Success = result;
                    response.Message = result ? string.Empty : "Something went wrong :| Please try later";
                }
            }
            return response;
        }

        public ServiceResponse<UserDto> GetUser(string userId)
        {
            var response = new ServiceResponse<UserDto>();
            var existingContact = _authRepository.GetUser(userId);
            if (existingContact != null)
            {
                var user = new UserDto()
                {
                    userId = existingContact.userId,
                    FirstName = existingContact.FirstName,
                    LastName = existingContact.LastName,
                    LoginId = userId,
                    ContactNumber = existingContact.ContactNumber,
                    Email = existingContact.Email,
                    FileName = existingContact.FileName,
                    ImageByte = existingContact.ImageByte

                };
                response.Data = user;
            }

            else
            {
                response.Success = false;
                response.Message = "Something went wrong,try after sometime";
            }
            return response;
        }


        public ServiceResponse<string> ModifyUser(User user, int userId, string email)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (_authRepository.UserExist(userId,user.LoginId, email))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;

            }

            var existingContact = _authRepository.GetUser(user.LoginId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = user.FirstName;
                existingContact.LastName = user.LastName;
                existingContact.ContactNumber = user.ContactNumber;
                existingContact.FileName = user.FileName;
                existingContact.ImageByte = user.ImageByte;
                result = _authRepository.UpdateUser(existingContact);
            }
            if (result)
            {
                response.Message = "User updated successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong,try after sometime";
            }
            return response;

        }


        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();
            string message = string.Empty;
            if (login != null)
            {
                var user = _authRepository.ValidateUser(login.Username);
                if (user == null || !_passwordService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                string token = _passwordService.CreateToken(user);
                response.Data = token;
                return response;

            }

            response.Success = false;
            response.Message = "Something went wrong";
            return response;
        }

        public ServiceResponse<string> ForgetPasswordService(ForgetDto forgetDto)
        {
            var response = new ServiceResponse<string>();

            if (forgetDto != null)
            {
                var user = _authRepository.ValidateUser(forgetDto.UserName);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username!";
                    return response;
                }

                var message = CheckPasswordStrength(forgetDto.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }

                if (forgetDto.Password != forgetDto.ConfirmPassword)
                {
                    response.Success = false;
                    response.Message = "Password and confirmation password do not match!";
                    return response;
                }

                // Create password hash and salt
                CreatePasswordHash(forgetDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Update user's password hash and salt
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _authRepository.UpdateUser(user); // Update the user with the new password hash and salt

                response.Success = true;
                response.Message = "Password reset successfully!";
                return response;
            }

            response.Success = false;
            response.Message = "Something went wrong, please try again later.";
            return response;
        }


        [ExcludeFromCodeCoverage]
        private string CheckPasswordStrength(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (password.Length < 8)
            {
                stringBuilder.Append("Minimum password length should be 8" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,*,(,),_,]"))
            {
                stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            }

            return stringBuilder.ToString();

        }


        [ExcludeFromCodeCoverage]
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
