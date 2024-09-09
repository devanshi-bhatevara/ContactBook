
using ContactBookApi.Data.Contract;
using ContactBookApi.Data.Implementation;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ContactBookApi.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;

        }

        public ServiceResponse<IEnumerable<ContactDto>> GetContacts(char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAll(letter);
            if (contacts != null && contacts.Any())
            {
                contacts.Where(c => c.FileName == string.Empty).ToList();
                List<ContactDto> contactDtos = new List<ContactDto>();

                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Address = contact.Address,
                        FileName = contact.FileName,
                        ImageByte = contact.ImageByte,
                        Gender = contact.Gender,
                        IsFavourite = contact.IsFavourite,
                        CountryId = contact.CountryId,
                        StateId = contact.StateId,
                        Country = new Country()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName
                        },
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName
                        },
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAllFavouriteContacts(letter);
            if (contacts != null && contacts.Any())
            {
                contacts.Where(c => c.FileName == string.Empty).ToList();
                List<ContactDto> contactDtos = new List<ContactDto>();

                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Address = contact.Address,
                        FileName = contact.FileName,
                        ImageByte = contact.ImageByte,
                        Gender = contact.Gender,
                        IsFavourite = contact.IsFavourite,
                        CountryId = contact.CountryId,
                        StateId = contact.StateId,
                        Country = new Country()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName
                        },
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName
                        },
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }

        //public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, string sortOrder)
        //{
        //    var response = new ServiceResponse<IEnumerable<ContactDto>>();
        //    var contacts = _contactRepository.GetPaginatedContacts(page, pageSize, sortOrder);
        
        //    if (contacts != null && contacts.Any())
        //    {
        //        List<ContactDto> contactDtos = new List<ContactDto>();
        //        foreach (var contact in contacts.ToList())
        //        {
        //            contactDtos.Add(new ContactDto()
        //            {
        //                ContactId = contact.ContactId,
        //                FirstName = contact.FirstName,
        //                LastName = contact.LastName,
        //                Phone = contact.Phone,
        //                Address = contact.Address,
        //                FileName = contact.FileName,
        //                ImageByte = contact.ImageByte,
        //                Email = contact.Email,
        //                Gender = contact.Gender,
        //                IsFavourite = contact.IsFavourite,
        //                CountryId = contact.CountryId,
        //                StateId = contact.StateId,
        //                Country = new Country()
        //                {
        //                    CountryId = contact.Country.CountryId,
        //                    CountryName = contact.Country.CountryName
        //                },
        //                State = new State()
        //                {
        //                    StateId = contact.State.StateId,
        //                    StateName = contact.State.StateName
        //                },

        //            });
        //        }


        //        response.Data = contactDtos;
        //        response.Success = true;
        //        response.Message = "Success";
        //    }
        //    else
        //    {
        //        response.Success = false;
        //        response.Message = "No record found";
        //    }

        //    return response;
        //}
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter,string? search,string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContacts(page, pageSize, letter,search, sortOrder);

            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Phone = contact.Phone,
                        Address = contact.Address,
                        FileName = contact.FileName,
                        ImageByte = contact.ImageByte,
                        Email = contact.Email,
                        Gender = contact.Gender,
                        IsFavourite = contact.IsFavourite,
                        CountryId = contact.CountryId,
                        StateId = contact.StateId,
                        Country = new Country()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName
                        },
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName
                        },
                        birthDate = contact.birthDate

                    });
                }


                response.Data = contactDtos;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }

        public ServiceResponse<IEnumerable<ContactSPDto>> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
            var contacts = _contactRepository.GetPaginatedContactsSP(page, pageSize, letter, search, sortOrder);

            if (contacts != null && contacts.Any())
            {
                response.Data = contacts;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactSPDto>> GetContactsBasedOnBirthdayMonth(int month)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
            var contacts = _contactRepository.GetContactsBasedOnBirthdayMonth(month);

            if (contacts != null && contacts.Any())
            {
                response.Data = contacts;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }  
        
        public ServiceResponse<IEnumerable<ContactSPDto>> GetContactByState(int state)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
            var contacts = _contactRepository.GetContactByState(state);

            if (contacts != null && contacts.Any())
            {
                response.Data = contacts;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        
        public ServiceResponse<int> GetContactsCountBasedOnCountry(int countryId)
        {
            var response = new ServiceResponse<int>();
            var contacts = _contactRepository.GetContactsCountBasedOnCountry(countryId);

            if (contacts > 0)
            {
                response.Data = contacts;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }   
        
        public ServiceResponse<int> GetContactsCountBasedOnGender(string gender)
        {
            var response = new ServiceResponse<int>();
            var contacts = _contactRepository.GetContactsCountBasedOnGender(gender);

            if (contacts > 0)
            {
                response.Data = contacts;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }

        public ServiceResponse<int> TotalContacts(char? letter, string? search)
        {
            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalContacts(letter, search);

            response.Data = totalPositions;
            return response;
        }  
        
        //public ServiceResponse<IEnumerable<ContactDto>> GetFavouritePaginatedContacts(int page, int pageSize)
        //{
        //    var response = new ServiceResponse<IEnumerable<ContactDto>>();
        //    var contacts = _contactRepository.GetFavouritePaginatedContacts(page, pageSize);

        //    if (contacts != null && contacts.Any())
        //    {
        //        List<ContactDto> contactDtos = new List<ContactDto>();
        //        foreach (var contact in contacts.ToList())
        //        {
        //            contactDtos.Add(new ContactDto()
        //            {
        //                ContactId = contact.ContactId,
        //                FirstName = contact.FirstName,
        //                LastName = contact.LastName,
        //                Phone = contact.Phone,
        //                Address = contact.Address,
        //                FileName = contact.FileName,
        //                ImageByte = contact.ImageByte,

        //                Email = contact.Email,
        //                Gender = contact.Gender,
        //                IsFavourite = contact.IsFavourite,
        //                CountryId = contact.CountryId,
        //                StateId = contact.StateId,
        //                Country = new Country()
        //                {
        //                    CountryId = contact.Country.CountryId,
        //                    CountryName = contact.Country.CountryName
        //                },
        //                State = new State()
        //                {
        //                    StateId = contact.State.StateId,
        //                    StateName = contact.State.StateName
        //                },

        //            });
        //        }


        //        response.Data = contactDtos;
        //    }
        //    else
        //    {
        //        response.Success = false;
        //        response.Message = "No record found";
        //    }

        //    return response;
        //}
        public ServiceResponse<IEnumerable<ContactDto>> GetFavouritePaginatedContacts(int page, int pageSize, char? letter, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder);

            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Phone = contact.Phone,
                        Address = contact.Address,
                        FileName = contact.FileName,
                        ImageByte = contact.ImageByte,
                        Email = contact.Email,
                        Gender = contact.Gender,
                        IsFavourite = contact.IsFavourite,
                        CountryId = contact.CountryId,
                        StateId = contact.StateId,
                        Country = new Country()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName
                        },
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName
                        },
                        birthDate = contact.birthDate
                       
                    });
                }


                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }

        public ServiceResponse<int> TotalFavouriteContacts(char? letter)
        {
            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalFavouriteContacts(letter);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Pagination successful";

            return response;
        }

        public ServiceResponse<ContactDto> GetContact(int contactId)
        {
            var response = new ServiceResponse<ContactDto>();
            var existingContact = _contactRepository.GetContact(contactId);
            if (existingContact != null)
            {
                var contact = new ContactDto()
                {
                    ContactId = contactId,
                    FirstName = existingContact.FirstName,
                    LastName = existingContact.LastName,
                    Address = existingContact.Address,
                    Phone = existingContact.Phone,
                    Email = existingContact.Email,
                    FileName = existingContact.FileName,
                    ImageByte = existingContact.ImageByte,
                    Gender = existingContact.Gender,
                    IsFavourite = existingContact.IsFavourite,
                    CountryId = existingContact.CountryId,
                    StateId = existingContact.StateId,
                    Country = new Country()
                    {
                        CountryId = existingContact.Country.CountryId,
                        CountryName = existingContact.Country.CountryName
                    },
                    State = new State()
                    {
                        StateId = existingContact.State.StateId,
                        StateName = existingContact.State.StateName
                    },
                    birthDate = existingContact.birthDate


                };
                response.Data = contact;
            }

            else
            {
                response.Success = false;
                response.Message = "Something went wrong,try after sometime";
            }
            return response;
        }


        public ServiceResponse<string> AddContact(ContactBook contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExist(contact.Phone))
            {
                response.Success = false;
                response.Message = "Contact Already Exist";
                return response;
            }

            if (!ValidateEmail(contact.Email))
            {
                response.Success = false;
                response.Message = "Email should be in xyz@abc.com format only!";
                return response;
            }

            if (contact.Phone.Length != 10)
            {
                response.Success = false;
                response.Message = "Number should include be 10 digits";
                return response;
            }

            if(contact.birthDate>DateTime.Now)
            {
                response.Success = false;
                response.Message = "Birthdate can't be greater than today's date";
                return response;
            }
            
           

            var result = _contactRepository.InsertContact(contact);
            if (result)
            {
                response.Data = contact.ContactId.ToString();
                response.Success = true;
                response.Message = "Contact Saved Successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong. Please try later";
            }
            return response;
        }


        public ServiceResponse<string> ModifyContact(ContactBook contact)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (_contactRepository.ContactExist(contact.ContactId, contact.Phone))
            {
                response.Success = false;
                response.Message = "Contact already exists.";
                return response;

            }

            if (contact.Phone.Length != 10)
            {
                response.Success = false;
                response.Message = "Number should include be 10 digits";
                return response;
            }
            if (contact.birthDate != null && contact.birthDate > DateTime.Now)
            {
                response.Success = false;
                response.Message = "Birthdate can't be greater than today's date";
                return response;
            }

            var existingContact = _contactRepository.GetContact(contact.ContactId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Email = contact.Email;
                existingContact.Address = contact.Address;
                existingContact.Phone = contact.Phone;
                existingContact.FileName = contact.FileName;
                existingContact.Gender = contact.Gender;
                existingContact.ImageByte = contact.ImageByte;
                existingContact.IsFavourite = contact.IsFavourite;
                existingContact.CountryId = contact.CountryId;
                existingContact.StateId = contact.StateId;
                existingContact.birthDate = contact.birthDate;
                result = _contactRepository.UpdateContact(existingContact);
            }
            if (result)
            {
                response.Message = "Contact updated successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong,try after sometime";
            }
            return response;

        }

        public ServiceResponse<string> RemoveContact(int id)
        {
            var response = new ServiceResponse<string>();
            var result = _contactRepository.DeleteContact(id);

            if (result)
            {
                response.Message = "Contact deleted successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong";
            }

            return response;
        }

        [ExcludeFromCodeCoverage]
        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);

        }

    }
}
