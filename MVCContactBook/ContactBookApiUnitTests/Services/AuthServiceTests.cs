using AutoFixture;
using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using ContactBookApi.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiUnitTests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public void RegisterUserService_ReturnsSuccess_WhenValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(true);


            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var result = target.RegisterUserService(registerDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenPasswordIsWeak()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Minimum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "pass"
            };

            // Act
            var result = target.RegisterUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(stringBuilder.ToString(), result.Message);
        }
        [Fact]
        public void RegisterUserService_ReturnsUserExists()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var result = target.RegisterUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User already exists", result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public void RegisterUserService_ReturnsSomeThingWentWrong_WhenInValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(false);


            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var result = target.RegisterUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong :| Please try later", result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void LoginUserService_ReturnsSomethingWentWrong_WhenLoginDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

         
            // Act
            var result = target.LoginUserService(null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong", result.Message);
        
        } 
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsNull()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns<User>(null); 

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

         
            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);


        }
         [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenPasswordIsWrong()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash,user.PasswordSalt)).Returns(false); 

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

         
            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt),Times.Once);


        }        
        
        [Fact]
        public void LoginUserService_ReturnsResponse_WhenLoginIsSuccessful()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash,user.PasswordSalt)).Returns(true); 
            mockConfiguration.Setup(repo => repo.CreateToken(user)).Returns(""); 

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

         
            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt),Times.Once);
            mockConfiguration.Verify(repo => repo.CreateToken(user), Times.Once);


        }

        [Fact]
        public void ForgetPasswordService_ReturnsSomethingWentWrong_WhenForgetDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.ForgetPasswordService(null);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try again later.", result.Message);
            
        }
        
        [Fact]
        public void ForgetPasswordService_ReturnsInvalidUsername_WhenUserIsNull()
        {
            //Arrange
            var forgetDto = new ForgetDto
            {
                UserName = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Invalid username!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);

        }

        [Fact]
        public void ForgetPasswordService_ReturnsFailure_WhenPasswordIsWeak()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Minimum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var forgetDto = new ForgetDto
            {
                UserName = "username",
                Password = "pass"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);


            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(stringBuilder.ToString(), result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);

        }
        [Fact]
        public void ForgetPasswordService_ReturnsFailure_WhenPasswordsDontMatch()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var forgetDto = new ForgetDto
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password234"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);


            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Password and confirmation password do not match!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);

        }
        [Fact]
        public void ForgetPasswordService_ReturnsSuccess_WhenPasswordResetSuccessfully()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var forgetDto = new ForgetDto
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password@1234"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);
            mockAuthRepository.Setup(repo => repo.UpdateUser(user));

            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Password reset successfully!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);
            mockAuthRepository.Verify(repo => repo.UpdateUser(user), Times.Once);

        }

        [Fact]
        public void GetUserById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var loginId = "testuser";
            var existingUser = new User
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567890",
                Email = "john@example.com",
                FileName = "file1.txt",

            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(r => r.GetUser(loginId)).Returns(existingUser);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);



            // Act
            var actual = target.GetUser(loginId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(loginId, actual.Data.LoginId);
            Assert.Equal(existingUser.userId, actual.Data.userId);
            Assert.Equal(existingUser.FirstName, actual.Data.FirstName);
            Assert.Equal(existingUser.LastName, actual.Data.LastName);
            Assert.Equal(existingUser.ContactNumber, actual.Data.ContactNumber);
            Assert.Equal(existingUser.Email, actual.Data.Email);
            Assert.Equal(existingUser.FileName, actual.Data.FileName);
            Assert.Equal(existingUser.ImageByte, actual.Data.ImageByte);

            mockAuthRepository.Verify(r => r.GetUser(loginId), Times.Once);
        }
        [Fact]
        public void GetUserById_ReturnsFailureMessage_WhenUserDoesNotExist()
        {
            // Arrange
            var loginId = "nonexistentuser";

            User nullUser = null;

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();

            mockAuthRepository.Setup(r => r.GetUser(loginId)).Returns(nullUser);


            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var actual = target.GetUser(loginId);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Something went wrong,try after sometime", actual.Message);

            mockAuthRepository.Verify(r => r.GetUser(loginId), Times.Once);
        }

        [Fact]
        public void ModifyUser_UserExists_UpdateSuccessful()
        {
            // Arrange
            var userId = 1;
            var email = "email@example.com";
            var updateDto = new User
            {
                LoginId =  "john",
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567890",
                FileName = "file1.txt",

            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(r => r.UserExist(updateDto.userId,updateDto.LoginId, updateDto.Email)).Returns(false);
            mockAuthRepository.Setup(r => r.GetUser(updateDto.LoginId)).Returns(new User()); // Return a non-null user
            mockAuthRepository.Setup(r => r.UpdateUser(It.IsAny<User>())).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var actual = target.ModifyUser(updateDto,userId,email);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal("User updated successfully.", actual.Message);

            mockAuthRepository.Verify(r => r.UserExist(userId,updateDto.LoginId, email), Times.Once);
            mockAuthRepository.Verify(r => r.GetUser(updateDto.LoginId), Times.Once);
            mockAuthRepository.Verify(r => r.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void ModifyUser_UserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var email = "email@example.com";
            var updateDto = new User
            {
                LoginId = "testuser",
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567890",
                FileName = "file1.txt",

            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(r => r.UserExist(userId, updateDto.LoginId, email)).Returns(false);
            mockAuthRepository.Setup(r => r.GetUser(updateDto.LoginId)).Returns((User)null); // Return null for non-existing user

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var actual = target.ModifyUser(updateDto, userId, email);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong,try after sometime", actual.Message);

            mockAuthRepository.Verify(r => r.UserExist(userId, updateDto.LoginId, email), Times.Once);
            mockAuthRepository.Verify(r => r.GetUser(updateDto.LoginId), Times.Once);
            mockAuthRepository.Verify(r => r.UpdateUser(It.IsAny<User>()), Times.Never); // UpdateUser should not be called
        }

        [Fact]
        public void ModifyUser_UserAlreadyExists()
        {
            // Arrange
            var userId = 1;
            var email = "email@example.com";
            var updateDto = new User
            {
                LoginId = "testuser",
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567890",
                FileName = "file1.txt",

            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(r => r.UserExist(userId, updateDto.LoginId ,email)).Returns(true); // Simulate user already exists

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var actual = target.ModifyUser(updateDto,userId,email);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("User already exists.", actual.Message);

            mockAuthRepository.Verify(r => r.UserExist(userId, updateDto.LoginId, email), Times.Once);

        }


    }
}
