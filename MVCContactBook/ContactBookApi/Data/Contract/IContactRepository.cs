using ContactBookApi.Dtos;
using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface IContactRepository
    {
        public IEnumerable<ContactSPDto> GetContactsBasedOnBirthdayMonth(int month);

        public IEnumerable<ContactSPDto> GetContactByState(int state);

        public int GetContactsCountBasedOnCountry(int countryId);

        public int GetContactsCountBasedOnGender(string gender);

        IEnumerable<ContactBook> GetAll(char? letter);

        ContactBook? GetContact(int id);

        IEnumerable<ContactBook> GetAllFavouriteContacts(char? letter);

        int TotalFavouriteContacts(char? letter);

        IEnumerable<ContactBook> GetFavouritePaginatedContacts(int page, int pageSize, char? letter, string sortOrder);
        //IEnumerable<ContactBook> GetFavouritePaginatedContacts(int page, int pageSize);

        IEnumerable<ContactSPDto> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder);

        bool InsertContact(ContactBook contact);

        bool UpdateContact(ContactBook contact);

        bool DeleteContact(int id);

        bool ContactExist(string phone);

        bool ContactExist(int id, string phone);

        int TotalContacts(char? letter, string? search);


        IEnumerable<ContactBook> GetPaginatedContacts(int page, int pageSize, char? letter, string? search, string sortOrder);

        //IEnumerable<ContactBook> GetPaginatedContacts(int page, int pageSize, string sortOrder);
    }
}
