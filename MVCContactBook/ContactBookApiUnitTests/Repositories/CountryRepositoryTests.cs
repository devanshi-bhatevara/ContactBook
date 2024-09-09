using ContactBookApi.Data;
using ContactBookApi.Data.Implementation;
using ContactBookApi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ContactBookApiUnitTests.Repositories
{
    public class CountryRepositoryTests
    {
        [Fact]
        public void GetAll_ReturnsCountries_WhenCountriesExist()
        {
            var countriesList = new List<Country>
            {
              new Country{ CountryId=1, CountryName="Country 1"},
              new Country{ CountryId=2, CountryName="Country 2"},
             }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();

            // This line is setting up our fake database to act like a real one. When our program asks for all the categories,
            // our fake database will give it the list of categories we already set up. This helps us test our program's behavior
            // without needing a real database
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countriesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.countries).Returns(mockDbSet.Object);
            var target = new CountryRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(countriesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.countries, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once);

        }

        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoCountriesExist()
        {
            var countriesList = new List<Country>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countriesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.countries).Returns(mockDbSet.Object);
            var target = new CountryRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(countriesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.countries, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once);

        }

    }
}
