using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService _authService)
        {
            this._authService = _authService;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {

            var response = _authService.RegisterUserService(registerDto);
            return !response.Success ? BadRequest(response) : Ok(response);

        }


        [HttpGet("GetUserById/{id}")]

        public IActionResult GetUserById(string id)
        {
            var response = _authService.GetUser(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("ModifyUser")]

        public IActionResult UpdateUser(UpdateUserDto userDto)
        {
            var existingUser = _authService.GetUser(userDto.LoginId);
            if (existingUser.Data == null)
            {
                return BadRequest(existingUser);
            }

            var contact = new User()

            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                LoginId = userDto.LoginId,
                ContactNumber = userDto.ContactNumber,
                FileName = userDto.FileName,
                ImageByte = userDto.ImageByte,

            };

            var response = _authService.ModifyUser(contact, existingUser.Data.userId,existingUser.Data.Email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }

        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var response = _authService.LoginUserService(loginDto);
            return !response.Success ? BadRequest(response) : Ok(response);

        }

        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(ForgetDto forgetDto)
        {
            var response = _authService.ForgetPasswordService(forgetDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
    }
}