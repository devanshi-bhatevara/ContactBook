using AutoFixture;
using ContactBookApi.Data.Contract;
using ContactBookApi.Models;
using ContactBookApi.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiUnitTests.Services
{
    public class ContactServiceTests
    {
        [Fact]

        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            var contacts = new List<ContactBook>
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(null)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(null);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(null), Times.Once);
        }

        [Fact]
        public void GetContacts_Returns_WhenNoContactsExistAndLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactBook>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(null)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(null);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetAll(null), Times.Once);
        }

        [Fact]

        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            // Arrange
            var contacts = new List<ContactBook>
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(letter), Times.Once);
        }

        [Fact]
        public void GetContacts_Returns_WhenNoContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';

            // Arrange
            var contacts = new List<ContactBook>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetAll(letter), Times.Once);
        }

        [Fact]

        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            var contacts = new List<ContactBook>
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllFavouriteContacts(null)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(null);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetAllFavouriteContacts(null), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_Returns_WhenNoContactsExistAndLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactBook>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllFavouriteContacts(null)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(null);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetAllFavouriteContacts(null), Times.Once);
        }

        [Fact]

        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            // Arrange
            var contacts = new List<ContactBook>
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllFavouriteContacts(letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAllFavouriteContacts(letter), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_Returns_WhenNoContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';

            // Arrange
            var contacts = new List<ContactBook>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllFavouriteContacts(letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetAllFavouriteContacts(letter), Times.Once);
        }

        [Fact]

        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull_SearchIsNull()
        {

            // Arrange
            string sortOrder = "asc";
            var contacts = new List<ContactBook>
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
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize,null,null,sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, null, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, null, sortOrder), Times.Once);
        }   
        
        [Fact]

        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull_SearchIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            string search = "abc";
            var contacts = new List<ContactBook>
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
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize,null, search, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, null, search, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, search, sortOrder), Times.Once);
        }
        [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull_SearchIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, null, null, sortOrder)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize,null,null,sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, null, sortOrder), Times.Once);
        }
          [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull_SearchIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "abc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, null, search, sortOrder)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize,null, search, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, search, sortOrder), Times.Once);
        }

        [Fact]

        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull_SearchIsNull()
        {

            // Arrange
            string sortOrder = "asc";
            var contacts = new List<ContactBook>

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
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, null,sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter,null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter,null,sortOrder), Times.Once);
        } 
        [Fact]

        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull_SearchIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            string search = "abc";
            var contacts = new List<ContactBook>

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
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, search, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder), Times.Once);
        }
        [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull_SearchIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter,null,sortOrder)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter,null,sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter,null,sortOrder), Times.Once);
        }
         [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull_SearchIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            string search = "abc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, search, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder), Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNull_SearchIsNull()
        {
            var contacts = new List<ContactBook>
        {
            new ContactBook
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(null,null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(null,null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(null,null), Times.Once);
        } 
        
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNull_SearchIsNotNull()
        {
            string search = "abc";
            var contacts = new List<ContactBook>
        {
            new ContactBook
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(null,search)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(null, search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(null, search), Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNotNull_SearchIsNull()
        {
            var letter = 'c';
            var contacts = new List<ContactBook>
        {
            new ContactBook
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(letter,null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(letter,null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(letter,null), Times.Once);
        }
          [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNotNull_SearchIsNotNull()
        {
            var letter = 'c';
            string search = "abc";
            var contacts = new List<ContactBook>
        {
            new ContactBook
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(letter, search)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(letter, search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(letter, search), Times.Once);
        }

        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            string sortOrder = "desc";
            var contacts = new List<ContactBook>
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
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouritePaginatedContacts(page, pageSize,null, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouritePaginatedContacts(page, pageSize,null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouritePaginatedContacts(page, pageSize,null, sortOrder), Times.Once);
        }

        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "desc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouritePaginatedContacts(page, pageSize,null, sortOrder)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouritePaginatedContacts(page, pageSize,null, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetFavouritePaginatedContacts(page, pageSize,null, sortOrder), Times.Once);
        }

        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            var contacts = new List<ContactBook>
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
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNull()
        {
            var contacts = new List<ContactBook>
        {
            new ContactBook
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalFavouriteContacts(null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalFavouriteContacts(null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalFavouriteContacts(null), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNotNull()
        {
            var letter = 'c';
            var contacts = new List<ContactBook>
        {
            new ContactBook
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactBook
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalFavouriteContacts(letter)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalFavouriteContacts(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalFavouriteContacts(letter), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsContact_WhenContactExist()
        {
            // Arrange
            var contactId = 1;
            var contact = new ContactBook
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

            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(contactId)).Returns(contact);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(contactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsNoRecord_WhenNoContactsExist()
        {
            // Arrange
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(contactId)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(contactId);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Something went wrong,try after sometime", actual.Message);
            mockRepository.Verify(r => r.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsContactSavedSuccessfully_WhenContactisSaved()
        {
            var contact = new ContactBook()
            {
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

            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExist(contact.Phone)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(true);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Contact Saved Successfully", actual.Message);
            mockRepository.Verify(r => r.ContactExist(contact.Phone), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsSomethingWentWrong_WhenContactisNotSaved()
        {
            var contact = new ContactBook()
            {
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
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExist(contact.Phone)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(false);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong. Please try later", actual.Message);
            mockRepository.Verify(r => r.ContactExist(contact.Phone), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var contact = new ContactBook()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExist(contact.Phone)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact Already Exist", actual.Message);
            mockRepository.Verify(r => r.ContactExist(contact.Phone), Times.Once);

        } 
        
        [Fact]
        public void AddContact_ReturnsError_WhenEmailIsInvalid()
        {
            var contact = new ContactBook()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johnexample.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Email should be in xyz@abc.com format only!", actual.Message);

        } 
        
        [Fact]
        public void AddContact_ReturnsError_WhenPhoneIsInvalid()
        {
            var contact = new ContactBook()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "joh@nexample.com",
                Phone = "1234890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Number should include be 10 digits", actual.Message);

        }    
        
        [Fact]
        public void AddContact_ReturnsError_WhenDateIsInvalid()
        {
            var contact = new ContactBook()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "joh@nexample.com",
                Phone = "1234899990",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1,
                birthDate = DateTime.Now.AddDays(1)
            };


            var mockRepository = new Mock<IContactRepository>();

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Birthdate can't be greater than today's date", actual.Message);

        }

        [Fact]
        public void ModifyContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var contactId = 1;
            var contact = new ContactBook()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExist(contactId, contact.Phone)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact already exists.", actual.Message);
            mockRepository.Verify(r => r.ContactExist(contactId, contact.Phone), Times.Once);
        }  
        
        [Fact]
        public void ModifyContact_ReturnsInvalid_WhenPhineInvalid()
        {
            var contactId = 1;
            var contact = new ContactBook()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "12367890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExist(contactId, contact.Phone)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Number should include be 10 digits", actual.Message);
            mockRepository.Verify(r => r.ContactExist(contactId, contact.Phone), Times.Once);
        }   
        
        [Fact]
        public void ModifyContact_ReturnsInvalid_WhenDateInvalid()
        {
            var contactId = 1;
            var contact = new ContactBook()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1236788890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1,
                birthDate = DateTime.Now.AddDays(1)
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExist(contactId, contact.Phone)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Birthdate can't be greater than today's date", actual.Message);
            mockRepository.Verify(r => r.ContactExist(contactId, contact.Phone), Times.Once);
        }
        [Fact]
        public void ModifyContact_ReturnsSomethingWentWrong_WhenContactNotFound()
        {
            var contactId = 1;
            var existingContact = new ContactBook()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                FileName = "file1.txt",
                Gender = "Male",
                IsFavourite = true,
                CountryId = 1,
                StateId = 1

            };

            var updatedContact = new ContactBook()
            {
                ContactId = contactId,
                FirstName = "C1"
            };


            var mockRepository = new Mock<IContactRepository>();
            //mockRepository.Setup(r => r.ContactExist(contactId, updatedContact.Phone)).Returns(false);
            mockRepository.Setup(r => r.GetContact(updatedContact.ContactId)).Returns<IEnumerable<ContactBook>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(existingContact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong,try after sometime", actual.Message);
            //mockRepository.Verify(r => r.ContactExist(contactId, updatedContact.Phone), Times.Once);
            mockRepository.Verify(r => r.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnsUpdatedSuccessfully_WhenContactModifiedSuccessfully()
        {

            //Arrange
            var existingContact = new ContactBook
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
                StateId = 1

            };

            var updatedContact = new ContactBook
            {
                ContactId = 1,
                FirstName = "Contact 1",
                Phone = "1234567890",

            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExist(updatedContact.ContactId, updatedContact.Phone)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(true);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Contact updated successfully.", actual.Message);

            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);


            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }
        [Fact]
        public void ModifyContact_ReturnsError_WhenContactModifiedFails()
        {

            //Arrange
            var existingContact = new ContactBook
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

            };

            var updatedContact = new ContactBook
            {
                ContactId = 1,
                FirstName = "Contact 1",
                Phone = "1234567890",

            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExist(updatedContact.ContactId, updatedContact.Phone)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(false);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong,try after sometime", actual.Message);
            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }



        [Fact]
        public void RemoveContact_ReturnsDeletedSuccessfully_WhenDeletedSuccessfully()
        {
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(contactId)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(contactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Contact deleted successfully", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(contactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_SomethingWentWrong_WhenDeletionFailed()
        {
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(contactId)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(contactId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(contactId), Times.Once);
        }

    }
}
