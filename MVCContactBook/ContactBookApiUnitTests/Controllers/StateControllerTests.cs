using ContactBookApi.Controllers;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiUnitTests.Controllers
{
    public class StateControllerTests
    {
        [Fact]
        public void GetAllStates_ReturnsOkWithStates_WhenStateExists()
        {
            //Arrange
            var states = new List<State>
             {
            new State{StateId=1,StateName="State 1", CountryId= 1},
            new State{StateId=2,StateName="State 2", CountryId= 2},
            };

            var response = new ServiceResponse<IEnumerable<StateDto>>
            {
                Success = true,
                Data = states.Select(c => new StateDto { StateId = c.StateId, StateName = c.StateName, CountryId = c.CountryId }) // Convert to StateDto
            };

            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(c => c.GetStates()).Returns(response);

            //Act
            var actual = target.GetAllStates() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockStateService.Verify(c => c.GetStates(), Times.Once);
        }

        [Fact]
        public void GetAllStates_ReturnsNotFound_WhenNoStateExists()
        {
            //Arrange


            var response = new ServiceResponse<IEnumerable<StateDto>>
            {
                Success = false,
                Data = new List<StateDto>()

            };

            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(c => c.GetStates()).Returns(response);

            //Act
            var actual = target.GetAllStates() as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);

            mockStateService.Verify(c => c.GetStates(), Times.Once);
        }

        [Fact]
        public void GetStateByCountryId_ReturnsOkWithState_WhenStateExists()
        {
            //Arrange
            int countryId = 1;
            var states = new List<State>
            {
                new State { StateId = 1, StateName = "State 1", CountryId = 1 },
                new State { StateId = 2, StateName = "State 2", CountryId = 1 }
            };

            var response = new ServiceResponse<IEnumerable<StateDto>>
            {
                Success = true,
                Data = states.Select(c => new StateDto { StateId = c.StateId, StateName = c.StateName, CountryId = c.CountryId })
            };
            var mockCategoryService = new Mock<IStateService>();
            var target = new StateController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetStatesByCountryId(countryId)).Returns(response);

            //Act
            var actual = target.GetStatesByCountryId(countryId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCategoryService.Verify(c => c.GetStatesByCountryId(countryId), Times.Once);
        }

        [Fact]
        public void GetStateByCountryId_ReturnsNotFound_WhenStateNotExists()
        {
            //Arrange
            int countryId = 1;
     
            var response = new ServiceResponse<IEnumerable<StateDto>>
            {
                Success = false,
                Data = null
            };
            var mockCategoryService = new Mock<IStateService>();
            var target = new StateController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetStatesByCountryId(countryId)).Returns(response);

            //Act
            var actual = target.GetStatesByCountryId(countryId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCategoryService.Verify(c => c.GetStatesByCountryId(countryId), Times.Once);
        }

    }
}
