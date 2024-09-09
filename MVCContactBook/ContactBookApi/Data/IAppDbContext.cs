using ContactBookApi.Dtos;
using ContactBookApi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data
{
    public interface IAppDbContext : IDbContext
    {

        public DbSet<ContactBook> ContactBook { get; set; }

        public DbSet<User> users { get; set; }

        public DbSet<Country> countries { get; set; }

        public DbSet<State> states { get; set; }

        IQueryable<ContactSPDto> ContactListSP(char? letter, string? search, int page, int pageSize, string sortOrder);

        IQueryable<ContactSPDto> GetContactsBasedOnBirthdayMonth(int month);

        IQueryable<ContactSPDto> GetContactByState(int state);

        IQueryable<CountDto> GetContactsCountBasedOnCountry(int countryId);
        IQueryable<CountDto> GetContactsCountBasedOnGender(string gender);
    }
}
