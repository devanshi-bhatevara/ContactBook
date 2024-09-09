using AutoFixture;
using ContactBookClientApp.Controllers;
using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace ContactBookClientAppTests.Controllers
{
    public class ContactControllerTests
    {

        [Fact]
        public void Index_ReturnsEmptyList_WhenLetterIsNotNull()
        {
            // Arrange
            char? letter = 'f';
            int page = 1;
            int pageSize = 2;
            string? searchQuery = "f";
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(letter, searchQuery, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
               .Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Exactly(2));
            mockHttpClientService
                .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        [Fact]
        public void Index_ReturnsCorrectView_WhenSearchQueryIsNotEmpty_LetterIsNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string searchQuery = "testSearchQuery";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { };


            var allContacts = new List<ContactViewModel> {
              new ContactViewModel { ContactId =1, FirstName = "Test", LastName = "test"},
              new ContactViewModel { ContactId =2, FirstName = "Test1", LastName = "test1"},
            };
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var letterResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = allContacts
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
              .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                  It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
              .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3


            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
               .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                   It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(letterResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(letter, searchQuery, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<List<ContactViewModel>>(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
                .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService
              .Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Exactly(2));
        }

        [Fact]
        public void Index_ReturnsEmptyList_WhenLetterIsNull_SearchisNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string? searchQuery = null;
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
        .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
            It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
        .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3


            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(letter, searchQuery, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
              .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                  It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService
              .Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Exactly(2));

        }


        [Fact]
        public void Index_ReturnsEmptyView_WhenTotalCountIsZero()
        {
            // Arrange
            char? letter = 'A';
            int page = 1;
            int pageSize = 2;
            string searchQuery = "testSearchQuery";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();


            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 0 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(letter, searchQuery, page, pageSize, sortOrder) as ViewResult;
            var model = actual.Model as List<ContactViewModel>;

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(model);
            mockHttpClientService
             .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                 It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        [Fact]
        public void Index_ReturnsRedirectToIndex_WhenPageIsGreaterThanTotalPages()
        {
            // Arrange
            char? letter = 'A';
            int page = 4;
            int pageSize = 2;
            string searchQuery = "test";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.Index(letter, searchQuery, page, pageSize, sortOrder);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, redirectToActionResult.RouteValues["page"]);
            Assert.Equal(pageSize, redirectToActionResult.RouteValues["pageSize"]);
            Assert.Equal(letter, redirectToActionResult.RouteValues["letter"]);
            mockHttpClientService
              .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                   It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        [Fact]
        public void Index_ReturnsEmptyContactsList()
        {
            //Arrange
            char? letter = 'A';
            int page = 1;
            int pageSize = 2;
            string? searchQuery = null;
            var expectedContacts = new List<ContactViewModel>
            {
            };
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>()
            {
                Success = false,
                Data = null
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            // Set up HttpRequest and HttpContext
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var apiGetContactsUrl = "fakeEndPointContact/GetAllContactsByPagination/?letter=A&page=1&pageSize=2";
            var apiGetCountUrl = "fakeEndPointContact/GetContactsCount/?letter=A";

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(apiGetContactsUrl, HttpMethod.Get, mockHttpRequest.Object, null, 60))
                .Returns(expectedResponse);

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = 0
            };

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, mockHttpRequest.Object, null, 60))
                .Returns(expectedCountResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(letter, searchQuery, page, pageSize) as ViewResult;
            var model = actual.Model as IEnumerable<ContactViewModel>;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            Assert.Equal(expectedContacts.Count, model.Count());
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
           .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
               It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }
        [Fact]
        public void Index_ReturnsEmptyList_WhenSuccessIsFalse()
        {
            // Arrange
            char? letter = 'f';
            int page = 1;
            int pageSize = 2;
            string? searchQuery = "f";
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(letter, searchQuery, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
               .Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Exactly(2));
            mockHttpClientService
                .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        [Fact]
        public void Favourites_ReturnsEmptyList_WhenLetterIsNotNull()
        {
            // Arrange
            char? letter = 'f';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Favourites(letter, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
               .Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Exactly(2));
            mockHttpClientService
                .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        [Fact]
        public void Favourites_ReturnsCorrectView_LetterIsNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { }; // Empty list as expected result

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var allContacts = new List<ContactViewModel> {
              new ContactViewModel { ContactId =1, FirstName = "Test", LastName = "test"},
              new ContactViewModel { ContactId =2, FirstName = "Test1", LastName = "test1"},
            };
            var letterResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = allContacts
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
              .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                  It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
              .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3


            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
             .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                  It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                    .Returns(letterResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Favourites(letter, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<List<ContactViewModel>>(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
                .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService
              .Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Exactly(2));
        }

        [Fact]
        public void Favourites_ReturnsEmptyView_WhenTotalCountIsZero()
        {
            // Arrange
            char? letter = 'A';
            int page = 1;
            int pageSize = 2;
            string searchQuery = "testSearchQuery";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();


            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 0 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Favourites(letter, page, pageSize, sortOrder) as ViewResult;
            var model = actual.Model as List<ContactViewModel>;

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(model);
            mockHttpClientService
             .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                 It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        [Fact]
        public void Favourites_ReturnsRedirectToIndex_WhenPageIsGreaterThanTotalPages()
        {
            // Arrange
            char? letter = 'A';
            int page = 4;
            int pageSize = 2;
            string sortOrder = "asc";

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.Favourites(letter, page, pageSize, sortOrder);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Favourites", redirectToActionResult.ActionName);
            Assert.Equal(1, redirectToActionResult.RouteValues["page"]);
            Assert.Equal(pageSize, redirectToActionResult.RouteValues["pageSize"]);
            Assert.Equal(letter, redirectToActionResult.RouteValues["letter"]);
            mockHttpClientService
              .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                   It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        [Fact]
        public void Favourites_ReturnsEmptyContactsList()
        {
            //Arrange
            char? letter = 'A';
            int page = 1;
            int pageSize = 2;
            var expectedContacts = new List<ContactViewModel>
            {
            };
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>()
            {
                Success = false,
                Data = null
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            // Set up HttpRequest and HttpContext
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var apiGetContactsUrl = "fakeEndPointContact/GetAllContactsByPagination/?letter=A&page=1&pageSize=2";
            var apiGetCountUrl = "fakeEndPointContact/GetFavouriteContactsCount/?letter=A";

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(apiGetContactsUrl, HttpMethod.Get, mockHttpRequest.Object, null, 60))
                .Returns(expectedResponse);

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, mockHttpRequest.Object, null, 60))
                .Returns(expectedCountResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Favourites(letter, page, pageSize) as ViewResult;
            var model = actual.Model as IEnumerable<ContactViewModel>;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            Assert.Equal(expectedContacts.Count, model.Count());
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
           .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
               It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        [Fact]
        public void Favourites_ReturnsEmptyList_WhenSuccessIsFalse()
        {
            // Arrange
            char? letter = 'f';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImageUpload = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Favourites(letter, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService
               .Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Exactly(2));
            mockHttpClientService
                .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }


        [Fact]
        public void Create_ReturnsView()
        {
            //Arrange
            var mockWebHostEnviornment = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
        {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2 },
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnviornment.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;
            var model = actual.Model as AddContactViewModel;

            //Assert
            Assert.NotNull(model);
            Assert.NotNull(model.Country);
            Assert.Equal(countries.Count, model.Country.Count());
            Assert.NotNull(model.States);
            Assert.Equal(countries.Count, model.States.Count());
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Create_ReturnsView_CountriesNotFound()
        {
            //Arrange
            var mockWebHostEnviornment = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false,
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false,

            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnviornment.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;
            var model = actual.Model as AddContactViewModel;

            //Assert
            Assert.NotNull(model);
            Assert.Empty(model.Country);
            Assert.Empty(model.States);
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Create_ReturnsView_StatesNotFound()
        {
            //Arrange
            var mockWebHostEnviornment = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnviornment.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;
            var model = actual.Model as AddContactViewModel;

            //Assert
            Assert.NotNull(model);
            Assert.NotNull(model.Country);
            Assert.Equal(countries.Count, model.Country.Count());
            Assert.Empty(model.States);
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Create_ReturnsView_CountriesNotFound_StatesNotFound()
        {
            //Arrange
            var mockWebHostEnviornment = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var expectedResponseCountires = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false

            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false

            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountires);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnviornment.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;
            var model = actual.Model as AddContactViewModel;
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(model.Country);
            Assert.Empty(model.States);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Create_ContactSavedSuccessfully_RedirectToAction()
        {
            //Arrange
            var mockWebHostEnviornment = new Mock<IImageUpload>();
            var viewModel = new AddContactViewModel()
            {
                FirstName = "C1",
                LastName = "L1"

            };
            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
        {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2 },
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Contact saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {

                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnviornment.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.NotNull(viewModel.Country);
            Assert.NotNull(viewModel.States);
            Assert.Equal(countries.Count, viewModel.Country.Count);
            Assert.Equal(states.Count, viewModel.States.Count);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);


        }


        [Fact]
        public void Create_ContactFailedToSave_RedirectToAction_WithInvalidFile()
        {
            //Arrange

            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
        {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2 },
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            var viewModel = new AddContactViewModel { FirstName = "C1", File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.pdf"), FileName = "xyz.pdf" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
    .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);

            var mockHttpContext = new Mock<HttpContext>();
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        public void Create_ContactSavedSuccessfully_RedirectToAction_WithCorrectFile()
        {
            //Arrange

            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
        {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2 },
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            var viewModel = new AddContactViewModel { FirstName = "C1", File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg"), FileName = "xyz.jpg" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);
            var expectedServiceResponse = new ServiceResponse<string>
            {

                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        [Fact]
        public void Create_ContactFailedToSaveServiceResponseNull_RedirectToAction()
        {
            //Arrange
            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
        {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2 },
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };
            var viewModel = new AddContactViewModel { FirstName = "C1" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Null(target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Create_ContactFailed_WhenModelStateIsInvalid()
        {
            //Arrange
            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new AddContactViewModel()
            {
                FirstName = "C1",

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var countries = new List<CountryViewModel>
            {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
            {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var mockHttpContext = new Mock<HttpContext>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }


        [Fact]
        public void Create_ContactFailedToSave_ReturnView()
        {
            //Arrange
            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
        {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2 },
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };


            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new AddContactViewModel { FirstName = "C1", LastName = "D1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Create_ReturnsSomethingWentWrong_ReturnView()
        {
            //Arrange
            var countries = new List<CountryViewModel>
   {
       new CountryViewModel { CountryId =1, CountryName = "C1"},
       new CountryViewModel { CountryId =2, CountryName = "C2"},
    };

            var states = new List<StateViewModel>
   {
       new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
       new StateViewModel { CountryId =2, StateName = "C2", StateId = 2 },
    };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new AddContactViewModel { FirstName = "C1", LastName = "D1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong. Please try after sometime";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
.Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedResponseStates);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Details_ReturnsView_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var viewModel = new UpdateContactViewModel { ContactId = id, FirstName = "C1" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Data = viewModel,
                Success = true
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Details(id) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }


        [Fact]
        public void Details_ReturnsErrorDataNull_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Message = "",
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Details(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Details_ReturnsErrorMessageNull_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Message = null,
                Data = new UpdateContactViewModel { ContactId = id, FirstName = "C1" },
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Details(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Details_RedirectToAction_WhenServiceResponseNull()
        {
            // Arrange
            int id = 1;
            var expectedSuccessResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = null
            };
            var mockImageUpload = new Mock<IImageUpload>();

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedSuccessResponse);
            var mockTepDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTepDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = tempData,
            };

            // Act
            var actual = target.Details(id) as RedirectToActionResult;
            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Details_ReturnsRedirectToAction_WhenFails()
        {
            //Arrange
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage

            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Details(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Details_ReturnsRedirectToAction_SomethingWentWrong()
        {
            //Arrange
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong. Please try after sometime";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Details(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new UpdateContactViewModel()
            {
                ContactId = id,
                FirstName = "C1"
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var countries = new List<CountryViewModel>
            {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
            {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Data = viewModel,
                Success = true
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as ViewResult;

            //Assert
            var model = actual.Model as UpdateContactViewModel;
            Assert.NotNull(model);
            Assert.NotNull(model.Countries);
            Assert.Equal(countries.Count, model.Countries.Count);
            Assert.NotNull(model.States);
            Assert.Equal(states.Count, model.States.Count);
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccessAndCountriesAreNull()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();


            var viewModel = new UpdateContactViewModel()
            {
                ContactId = id,
                FirstName = "C1"
            };
            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false,
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false,
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();

            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Success = true,
                Data = viewModel
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as ViewResult;
            var model = actual.Model as UpdateContactViewModel;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Empty(model.Countries);
            Assert.Empty(model.States);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccessAndStatesAreNull()
        {
            var id = 1;
            var countries = new List<CountryViewModel>
            {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };
            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new UpdateContactViewModel()
            {
                ContactId = id,
                FirstName = "C1"
            };
            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };

            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false,
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();

            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Success = true,
                Data = viewModel
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as ViewResult;
            var model = actual.Model as UpdateContactViewModel;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(model.Countries.Count, countries.Count);
            Assert.Empty(model.States);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsErrorDataNull_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Message = "",
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsError_SuccessFalse_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Message = null,
                Data = new UpdateContactViewModel { ContactId = id, FirstName = "C1" },
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_RedirectToAction_WhenCountryNotExistsServiceResponseNull()
        {
            // Arrange
            int contactId = 1;
            var expectedSuccessResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = null
            };
            var mockImageUpload = new Mock<IImageUpload>();

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedSuccessResponse);
            var mockTepDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTepDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = tempData,
            };

            // Act
            var actual = target.Edit(contactId) as RedirectToActionResult;
            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Edit_ReturnsRedirectToAction_WhenFails()
        {
            //Arrange
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage

            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsRedirectToAction_SomethingWentWrong()
        {
            //Arrange
            var id = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockImageUpload = new Mock<IImageUpload>();

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong. Please try after sometime";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ContactSavedSuccessfully_RedirectToAction()
        {
            //Arrange
            var id = 1;
            var viewModel = new UpdateContactViewModel { ContactId = id, FirstName = "C1" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var countries = new List<CountryViewModel>
   {
   new CountryViewModel { CountryId =1, CountryName = "C1"},
   new CountryViewModel { CountryId =2, CountryName = "C2"},
};

            var states = new List<StateViewModel>
   {
   new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
   new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
};

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var successMessage = "Contact saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void Edit_ContactSavedSuccessfully_RedirectToAction_RemoveImage()
        {
            //Arrange
            var id = 1;

            var viewModel = new UpdateContactViewModel { ContactId = id, FirstName = "C1", File = new FormFile(new MemoryStream(new byte[1]), 0, 0, "xyz", "xyz.jpg"), FileName = "xyz.jpg", RemoveImage = true };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Contact saved successfully";
            var countries = new List<CountryViewModel>
   {
   new CountryViewModel { CountryId =1, CountryName = "C1"},
   new CountryViewModel { CountryId =2, CountryName = "C2"},
};

            var states = new List<StateViewModel>
   {
   new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
   new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
};

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Null(viewModel.FileName);
            Assert.Null(viewModel.ImageByte);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ContactFailedToSave_RedirectToAction_WithPdfFile()
        {
            //Arrange
            var id = 1;

            var viewModel = new UpdateContactViewModel { ContactId = id, FirstName = "C1", File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.pdf"), FileName = "xyz.pdf" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var countries = new List<CountryViewModel>
   {
   new CountryViewModel { CountryId =1, CountryName = "C1"},
   new CountryViewModel { CountryId =2, CountryName = "C2"},
};

            var states = new List<StateViewModel>
   {
   new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
   new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
};

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var errorMessage = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
            var mockHttpContext = new Mock<HttpContext>();
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ContactSavedSuccessfully_RedirectToAction_WithCorrectFile()
        {
            //Arrange
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "C1", File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg"), FileName = "xyz.jpg" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            var countries = new List<CountryViewModel>
   {
   new CountryViewModel { CountryId =1, CountryName = "C1"},
   new CountryViewModel { CountryId =2, CountryName = "C2"},
};

            var states = new List<StateViewModel>
   {
   new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
   new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
};

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var expectedServiceResponse = new ServiceResponse<string>
            {

                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }


        [Fact]
        public void Edit_ContactFailedToSaveServiceResponseNull_RedirectToAction()
        {
            //Arrange
            var id = 1;
            var viewModel = new UpdateContactViewModel { ContactId = id, FirstName = "C1" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var countries = new List<CountryViewModel>
   {
   new CountryViewModel { CountryId =1, CountryName = "C1"},
   new CountryViewModel { CountryId =2, CountryName = "C2"},
};

            var states = new List<StateViewModel>
   {
   new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
   new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
};

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(null, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Edit_ContactFailed_WhenModelStateIsInvalid()
        {
            //Arrange
            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new UpdateContactViewModel()
            {
                ContactId = 1,
                FirstName = "C1",

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var countries = new List<CountryViewModel>
            {
            new CountryViewModel { CountryId =1, CountryName = "C1"},
            new CountryViewModel { CountryId =2, CountryName = "C2"},
         };

            var states = new List<StateViewModel>
            {
            new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
            new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
         };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var mockHttpContext = new Mock<HttpContext>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));

            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ContactFailed_WhenModelStateIsInvalid_WhenStateCountryAreNull()
        {
            //Arrange
            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new UpdateContactViewModel()
            {
                ContactId = 1,
                FirstName = "C1",

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var mockHttpContext = new Mock<HttpContext>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void Edit_ContactFailedToSave_ReturnRedirectToActionResult()
        {
            //Arrange
            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new UpdateContactViewModel { FirstName = "C1", LastName = "D1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var countries = new List<CountryViewModel>
   {
   new CountryViewModel { CountryId =1, CountryName = "C1"},
   new CountryViewModel { CountryId =2, CountryName = "C2"},
};

            var states = new List<StateViewModel>
   {
   new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
   new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
};

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsSomethingWentWrong_ReturnRedirectToActionResult()
        {
            //Arrange
            var mockImageUpload = new Mock<IImageUpload>();
            var viewModel = new UpdateContactViewModel { FirstName = "C1", LastName = "D1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong. Please try after sometime";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var countries = new List<CountryViewModel>
   {
   new CountryViewModel { CountryId =1, CountryName = "C1"},
   new CountryViewModel { CountryId =2, CountryName = "C2"},
};

            var states = new List<StateViewModel>
   {
   new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
   new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
};

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        [Fact]
        public void Delete_ReturnsView_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var viewModel = new ContactViewModel { ContactId = id, FirstName = "C1" };
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Data = viewModel,
                Success = true
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Delete(id) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }


        [Fact]
        public void Delete_ReturnsErrorDataNull_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Message = "",
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Delete(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Delete_ReturnsErrorMessageNull_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Message = null,
                Data = new ContactViewModel { ContactId = id, FirstName = "C1" },
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Delete(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Delete_RedirectToAction_WhenServiceResponseNull()
        {
            // Arrange
            int id = 1;
            var expectedSuccessResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = null
            };
            var mockImageUpload = new Mock<IImageUpload>();

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedSuccessResponse);
            var mockTepDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTepDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = tempData,
            };

            // Act
            var actual = target.Delete(id) as RedirectToActionResult;
            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsRedirectToAction_WhenFails()
        {
            //Arrange
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage

            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Delete(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void Delete_ReturnsRedirectToAction_SomethingWentWrong()
        {
            //Arrange
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong. Please try after sometime";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Delete(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }

        [Fact]
        public void DeleteConfirm_ReturnsRedirectToAction_WhenDeletedSuccessfully()
        {
            // Arrange
            var id = 1;

            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = "Success",
                Success = true
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedServiceResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var actual = target.DeleteConfirm(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void DeleteConfirm_ReturnsRedirectToAction_WhenDeletionFailed()
        {
            // Arrange
            var id = 1;
            var mockImageUpload = new Mock<IImageUpload>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = "Error",
                Success = false
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedServiceResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var actual = target.DeleteConfirm(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

    }
}
