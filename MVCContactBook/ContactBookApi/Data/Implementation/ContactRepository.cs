using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly IAppDbContext _appDbContext;


        public ContactRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }
        public IEnumerable<ContactBook> GetAll(char? letter)
        {

            List<ContactBook> contacts = _appDbContext.ContactBook
                .Include(c=>c.Country)
                .Include(c=>c.State)
                .Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            return contacts;
        }
        public IEnumerable<ContactBook> GetAllFavouriteContacts(char? letter)
        {

            List<ContactBook> contacts = _appDbContext.ContactBook
                .Include(c=>c.Country)
                .Include(c=>c.State)
                .Where(c => c.IsFavourite)
                .Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            return contacts;
        }

        public ContactBook? GetContact(int id)
        {
            var contact = _appDbContext.ContactBook
                .Include(c=>c.Country)
                .Include(c=>c.State)
                .FirstOrDefault(c => c.ContactId == id);
            return contact;
        }

        public int TotalContacts(char? letter, string? search)
        {
            IQueryable<ContactBook> query = _appDbContext.ContactBook;
         
            if (letter != null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));

            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }

            return query.Count();
        }
        public IEnumerable<ContactBook> GetPaginatedContacts(int page, int pageSize, char? letter,string? search, string sortOrder)
        {
            int skip = (page - 1) * pageSize;
            IQueryable<ContactBook> query = _appDbContext.ContactBook
                .Include(c => c.State)
                .Include(c => c.Country);

            if (letter != null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));

            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }

            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    break;
                default:
                    query = query.OrderBy(c => c.FirstName);
                    break;
            }

            return query
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<ContactSPDto> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder)
        {
            var result = _appDbContext.ContactListSP(letter, search, page, pageSize, sortOrder);
            return result;
        }

        public IEnumerable<ContactSPDto> GetContactsBasedOnBirthdayMonth(int month)
        {
            var result = _appDbContext.GetContactsBasedOnBirthdayMonth(month);
            return result;
        } 
        
        public IEnumerable<ContactSPDto> GetContactByState(int state)
        {
            var result = _appDbContext.GetContactByState(state);
            return result;
        }  
        
        public int GetContactsCountBasedOnCountry(int countryId)
        {
            var result = _appDbContext.GetContactsCountBasedOnCountry(countryId).ToList().FirstOrDefault().CountOfContacts;
            return result;
        } 
        
        public int GetContactsCountBasedOnGender(string gender)
        {
            var result = _appDbContext.GetContactsCountBasedOnGender(gender).ToList().FirstOrDefault().CountOfContacts;
            return result;
        } 

        public int TotalFavouriteContacts(char? letter)
        {
            IQueryable<ContactBook> query = _appDbContext.ContactBook.Where(c=>c.IsFavourite);

            if (letter.HasValue)
            {
                query = query.Where(c=>c.IsFavourite).Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }

        public IEnumerable<ContactBook> GetFavouritePaginatedContacts(int page, int pageSize, char? letter,string sortOrder)
        {
            int skip = (page - 1) * pageSize;

            IQueryable<ContactBook> query = _appDbContext.ContactBook
                .Include(c => c.Country)
                .Include(c => c.State)
                .Where(c => c.IsFavourite)
                .Where(c => letter == null || c.FirstName.StartsWith(letter.ToString()));

            // Apply sorting based on the sortOrder parameter
            switch (sortOrder)
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName);
                    break;
                default:
                    // Default sorting order if sortOrder is not specified
                    query = query.OrderBy(c => c.FirstName);
                    break;
            }

            return query.Skip(skip)
                        .Take(pageSize)
                        .ToList();
        }
    

        public bool InsertContact(ContactBook contact)
        {
            var result = false;
            if (contact != null)
            {
                _appDbContext.ContactBook.Add(contact);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool UpdateContact(ContactBook contact)
        {
            var result = false;
            if (contact != null)
            {
                _appDbContext.ContactBook.Update(contact);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteContact(int id)
        {
            var result = false;
            var contact = _appDbContext.ContactBook.Find(id);
            if (contact != null)
            {
                _appDbContext.ContactBook.Remove(contact);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool ContactExist(string phone)
        {
            var contact = _appDbContext.ContactBook.FirstOrDefault(c => c.Phone == phone);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContactExist(int id, string phone)
        {
            var contact = _appDbContext.ContactBook.FirstOrDefault(c => c.Phone == phone && c.ContactId != id);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
