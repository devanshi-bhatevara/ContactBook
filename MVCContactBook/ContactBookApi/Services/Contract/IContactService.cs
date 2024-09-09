
using ContactBookApi.Dtos;
using ContactBookApi.Models;

namespace ContactBookApi.Services.Contract
{
    public interface IContactService
    {
        ServiceResponse<IEnumerable<ContactDto>> GetContacts(char? letter);

        ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(char? letter);
        public ServiceResponse<ContactDto> GetContact(int contactId);

        //ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize,string sortOrder);

        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter,string? search, string sortOrder);

        ServiceResponse<IEnumerable<ContactSPDto>> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder);
        ServiceResponse<int> TotalContacts(char? letter, string? search);

        //public ServiceResponse<IEnumerable<ContactDto>> GetFavouritePaginatedContacts(int page, int pageSize);
        public ServiceResponse<IEnumerable<ContactDto>> GetFavouritePaginatedContacts(int page, int pageSize, char? letter, string sortOrder);

        public ServiceResponse<int> TotalFavouriteContacts(char? letter);
        public ServiceResponse<string> AddContact(ContactBook contact);


        public ServiceResponse<string> ModifyContact(ContactBook contact);

        public ServiceResponse<string> RemoveContact(int id);

        public ServiceResponse<IEnumerable<ContactSPDto>> GetContactsBasedOnBirthdayMonth(int month);

        public ServiceResponse<IEnumerable<ContactSPDto>> GetContactByState(int state);

        public ServiceResponse<int> GetContactsCountBasedOnCountry(int countryId);

        public ServiceResponse<int> GetContactsCountBasedOnGender(string gender);
    }
}
