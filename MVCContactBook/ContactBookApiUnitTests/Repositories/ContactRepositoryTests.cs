using ContactBookApi.Data;
using ContactBookApi.Data.Implementation;
using ContactBookApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiUnitTests.Repositories
{
    public class ContactRepositoryTests
    {
        [Fact]
        public void GetAll_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {
            char? letter = null;
            var contactsList = new List<ContactBook>
            {
               new ContactBook
             {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Phone = "9876543210",
                Address = "456 Elm St",
                FileName = "file2.txt",
                Gender = "Female",
                IsFavourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());
        }
        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;

            var contactsList = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());

        }
        [Fact]
        public void GetAll_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            char? letter = 'J';
            var contactsList = new List<ContactBook>
            {
               new ContactBook
             {
                ContactId = 1,
                FirstName = "john",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Phone = "9876543210",
                Address = "456 Elm St",
                FileName = "file2.txt",
                Gender = "Female",
                IsFavourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());

        }
        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoContactsExistLetterIsNotNull()
        {
            char? letter = 'a';

            var contactsList = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());

        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {
            var contactsList = new List<ContactBook>
            {
               new ContactBook
             {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Phone = "9876543210",
                Address = "456 Elm St",
                FileName = "file2.txt",
                Gender = "Female",
                IsFavourite = true,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllFavouriteContacts(null);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());
        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsEmpty_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;

            var contactsList = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllFavouriteContacts(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());

        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            char? letter = 'J';
            var contactsList = new List<ContactBook>
            {
               new ContactBook
             {
                ContactId = 1,
                FirstName = "john",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Phone = "9876543210",
                Address = "456 Elm St",
                FileName = "file2.txt",
                Gender = "Female",
                IsFavourite = true,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllFavouriteContacts(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());

        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsEmpty_WhenNoContactsExistLetterIsNotNull()
        {
            char? letter = 'a';

            var contactsList = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllFavouriteContacts(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once());

        }

        [Fact]
        public void GetContactBook_WhenContactBookIsNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.ContactBook, Times.Once);

        }
        [Fact]
        public void GetContactBook_WhenContactBookIsNotNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<ContactBook>()
            {
              new ContactBook { ContactId = 1, FirstName = "ContactBook 1" },
                new ContactBook { ContactId = 2, FirstName = "ContactBook 2" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNull_SearchIsNull()
        {
            char? letter = null;
            var contacts = new List<ContactBook> {
                new ContactBook {ContactId = 1,FirstName="Contact 1"},
                new ContactBook {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }
        
        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNull_SearchIsNotNull()
        {
            char? letter = null;
            string search = "c";
            var contacts = new List<ContactBook> {
                new ContactBook {ContactId = 1,FirstName="Contact 1"},
                new ContactBook {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull_SearchIsNull()
        {
            char? letter = null;
            var contacts = new List<ContactBook>
            {

            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter,null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }
        
        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull_SearchIsNotNull()
        {
            char? letter = null;
            var contacts = new List<ContactBook>
            {

            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            string search = "abc";
            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull_SearchIsNull()
        {
            char? letter = 'c';
            var contacts = new List<ContactBook> {
                new ContactBook {ContactId = 1,FirstName="Contact 1"},
                new ContactBook {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter,null);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }
       
        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull_SearchIsNotNull()
        {
            char? letter = 'c';
            string search = "abc";
            var contacts = new List<ContactBook> {
                new ContactBook {ContactId = 1,FirstName="Contact 1"},
                new ContactBook {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull_SearchIsNull()
        {
            char? letter = 'c';
            var contacts = new List<ContactBook>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter,null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }
        
        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull_SearchIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<ContactBook>
            {

            }.AsQueryable();
            string search = "abc";
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCount_WhenContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<ContactBook> {
                new ContactBook {ContactId = 1,FirstName="Contact 1"},
                new ContactBook {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavouriteContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<ContactBook>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavouriteContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<ContactBook> {
                new ContactBook {ContactId = 1,FirstName="Contact 1"},
                new ContactBook {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavouriteContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<ContactBook>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavouriteContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.ContactBook, Times.Once);

        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExists_LetterIsNull_SearchIsNull()
        {
            string sortOrder = "asc";
            var contacts = new List<ContactBook>
              {
                  new ContactBook{ContactId=1, FirstName="Contact 1"},
                  new ContactBook{ContactId=2, FirstName="Contact 2"},
                  new ContactBook{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null,null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }
       
        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExists_LetterIsNull_SearchIsNotNull()
        {
            string sortOrder = "asc";
            string search = "C";
            var contacts = new List<ContactBook>
              {
                  new ContactBook{ContactId=1, FirstName="Contact 1"},
                  new ContactBook{ContactId=2, FirstName="Contact 2"},
                  new ContactBook{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExists_LetterIsNull_SearchIsNull()
        {
            string sortOrder = "asc";
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }     
        
        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExists_LetterIsNull_SearchIsNotNull()
        {
            string search = "con";
            string sortOrder = "asc";
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter_SearchIsNull()
        {
            char letter = 'c';
            string sortOrder = "desc";
            var contacts = new List<ContactBook>
              {
                  new ContactBook{ContactId=1, FirstName="Contact 1"},
                  new ContactBook{ContactId=2, FirstName="Contact 2"},
                  new ContactBook{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter,null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }  
        
        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter_SearchIsNotNull()
        {
            char letter = 'c';
            string sortOrder = "ghgghjn";
            string search = "con";
            var contacts = new List<ContactBook>
              {
                  new ContactBook{ContactId=1, FirstName="Contact 1"},
                  new ContactBook{ContactId=2, FirstName="Contact 2"},
                  new ContactBook{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter_SearchIsNull()
        {
            char letter = 'c';
            string sortOrder = "desc";
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter,null,sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }  
        
        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter_SearchIsNotNull()
        {
            char letter = 'c';
            string sortOrder = "desc";
            string search = "c";
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter,search,sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsCorrectContacts_WhenContactsExists()
        {
            string sortOrder = "desc";
            var contacts = new List<ContactBook>
              {
                  new ContactBook{ContactId=1, FirstName="Contact 1"},
                  new ContactBook{ContactId=2, FirstName="Contact 2"},
                  new ContactBook{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouritePaginatedContacts(1, 2,null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsEmptyList_WhenNoContactsExists()
        {
            string sortOrder = "asc";
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouritePaginatedContacts(1, 2,null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter()
        {
            string sortOrder = "desc";

            char letter = 'c';
            var contacts = new List<ContactBook>
              {
                  new ContactBook{ContactId=1, FirstName="Contact 1"},
                  new ContactBook{ContactId=2, FirstName="Contact 2"},
                  new ContactBook{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouritePaginatedContacts(1, 2, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter()
        {
            char letter = 'c';
            string sortOrder = "hii";
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouritePaginatedContacts(1, 2, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void InsertContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.ContactBook).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new ContactBook
            {
                ContactId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.InsertContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void InsertContact_ReturnsFalse()
        {
            //Arrange
            ContactBook contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.InsertContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void UpdateContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.ContactBook).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new ContactBook
            {
                ContactId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.UpdateContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void UpdateContact_ReturnsFalse()
        {
            //Arrange
            ContactBook contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.UpdateContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void DeleteContact_ReturnsTrue()
        {
            // Arrange
            var contactId = 1;
            var contact = new ContactBook { ContactId = contactId };
            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.ContactBook.Find(contactId)).Returns(contact);
            var target = new ContactRepository(mockContext.Object);
            // Act
            var result = target.DeleteContact(contactId);

            // Assert
            Assert.True(result);
            mockContext.Verify(c => c.ContactBook.Remove(contact), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            mockContext.Verify(c => c.ContactBook.Find(contactId), Times.Once);

        }

        [Fact]
        public void DeleteContact_ReturnsFalse()
        {
            //Arrange
            var id = 1;
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.DeleteContact(id);
            //Assert
            Assert.False(actual);
            mockAbContext.VerifyGet(c => c.ContactBook, Times.Once);
        }

        [Fact]
        public void ContactExists_ReturnsTrue()
        {
            //Arrange
            var phone = "1234567890";
            var contacts = new List<ContactBook>
            {
                new ContactBook { ContactId = 1, FirstName = "Contact 1", Phone="1234567890"},
                new ContactBook { ContactId = 2, FirstName = "Contact 2", Phone="9876543216" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExist(phone);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
        }

        [Fact]
        public void ContactExists_ReturnsFalse()
        {
            //Arrange
            var phone = "1234567890";
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExist(phone);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
        }

        [Fact]
        public void ContactExistsIdName_ReturnsFalse()
        {
            //Arrange
            var phone = "1234567890";
            var id = 1;
            var contacts = new List<ContactBook>().AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExist(id, phone);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
        }

        [Fact]
        public void ContactExistsIdName_ReturnsTrue()
        {
            //Arrange
            var phone = "1234567890";
            var id = 3;
            var contacts = new List<ContactBook>
            {
                new ContactBook { ContactId = 1, FirstName = "Contact 1", Phone="1234567890" },
                new ContactBook { ContactId = 2, FirstName = "Contact 2" , Phone="9876543219"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<ContactBook>>();
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<ContactBook>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.ContactBook).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExist(id, phone);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<ContactBook>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.ContactBook, Times.Once);
        }

    }
}
