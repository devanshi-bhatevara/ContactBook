using AutoFixture;
using ContactBookApi.Controllers;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiUnitTests.Controllers
{
    public class AuthControllerTests
    {
        [Theory]
        [InlineData("User already exists.")]
        [InlineData("Something went wrong, please try after sometime.")]
        [InlineData("Mininum password length should b e 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        public void Register_ReturnsBadRequest_WhenRegistrationFails(string message)
        {
            // Arrange
            var registerDto = new RegisterDto();
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }

        [Theory]
        [InlineData("Invalid username or password!")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }

        [Fact]
        public void Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                LoginId = "loginid",
                Email = "email@email.com",
                Password = "password",
                ConfirmPassword = "password",
                ContactNumber = "1234567890",
                FirstName = "firstname",
                LastName = "lastname"
            };
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var controller = new AuthController(mockAuthService.Object);

            // Act
            var actual = controller.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }

        [Fact]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }

        [Theory]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        [InlineData("Invalid username!")]
        [InlineData("Password and confirmation password do not match!")]
        [InlineData("Something went wrong, please try again later.")]
        public void ForgetPassword_ReturnsBadRequest_WhenForgetPasswordFails(string message)
        {
            // Arrange
            var forgetDto = new ForgetDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ForgetPassword(forgetDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }

        [Fact]
        public void ForgetPassword_ReturnsOk_WhenForgetPasswordSucceeds()
        {
            // Arrange
            var fixture = new Fixture();
            var forgetDto = new ForgetDto()
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password@1234"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = ""

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ForgetPassword(forgetDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }

        [Fact]
        public void GetUserByLogInId_ReturnsOk()
        {
            // Arrange
            var loginId = "testuser";
            var response = new ServiceResponse<UserDto>
            {
                Success = true,
                Data = new UserDto
                {
                    LoginId = loginId,
                    userId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    ContactNumber = "1234567890",
                    FileName = "file1.txt",

                }
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(s => s.GetUser(loginId)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var result = target.GetUserById(loginId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(response, result.Value);
            mockAuthService.Verify(s => s.GetUser(loginId), Times.Once);
        }

        [Fact]
        public void GetUserByLogInId_ReturnsNotFound()
        {
            // Arrange
            var loginId = "nonexistentuser";
            var response = new ServiceResponse<UserDto>
            {
                Success = false,
                Message = "User not found",
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(s => s.GetUser(loginId)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var result = target.GetUserById(loginId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(response, result.Value);
            mockAuthService.Verify(s => s.GetUser(loginId), Times.Once);
        }

        [Fact]
        public void UpdateContact_ReturnsOk_WhenContactIsUpdatedSuccessfully()
        {
            // Arrange

            var fixture = new Fixture();
            var updateRegisterDto = fixture.Create<UpdateUserDto>();
            var user = fixture.Create<UserDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
                Message = "User updated successfully."
            };
            var userResponse = new ServiceResponse<UserDto>
            {
                Success = true,
                Data = user
            };

           

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(s => s.GetUser(updateRegisterDto.LoginId)).Returns(userResponse);
            mockAuthService.Setup(s => s.ModifyUser(It.IsAny<User>(),userResponse.Data.userId,userResponse.Data.Email)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.UpdateUser(updateRegisterDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(s => s.ModifyUser(It.IsAny<User>(), userResponse.Data.userId, userResponse.Data.Email), Times.Once);
            mockAuthService.Verify(s => s.GetUser(updateRegisterDto.LoginId), Times.Once);

        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenContactIsNotUpdated()
        {
            // Arrange

            var fixture = new Fixture();
            var updateRegisterDto = fixture.Create<UpdateUserDto>();
            var user = fixture.Create<UserDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = "Went Wrong"
            };
            var userResponse = new ServiceResponse<UserDto>
            {
                Success = true,
                Data = user
            };

           

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(s => s.GetUser(updateRegisterDto.LoginId)).Returns(userResponse);
            mockAuthService.Setup(s => s.ModifyUser(It.IsAny<User>(),userResponse.Data.userId,userResponse.Data.Email)).Returns(response);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.UpdateUser(updateRegisterDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(s => s.ModifyUser(It.IsAny<User>(), userResponse.Data.userId, userResponse.Data.Email), Times.Once);
            mockAuthService.Verify(s => s.GetUser(updateRegisterDto.LoginId), Times.Once);
        }
        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenContactIsNotFound()
        {
            // Arrange

            var fixture = new Fixture();
            var updateRegisterDto = fixture.Create<UpdateUserDto>();


          
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(s => s.GetUser(updateRegisterDto.LoginId)).Returns<UserDto>(null);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.UpdateUser(updateRegisterDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            mockAuthService.Verify(s => s.GetUser(updateRegisterDto.LoginId), Times.Once);

        }







    }
}
