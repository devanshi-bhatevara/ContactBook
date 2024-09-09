using ContactBookApi.Dtos;
using ContactBookApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContactBook> ContactBook { get; set; }

        public DbSet<User> users { get; set; }

        public DbSet<Country> countries { get; set; }

        public DbSet<State> states { get; set; }

        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }

        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            
            modelBuilder.Entity<State>()
               .HasOne(p => p.Country)
               .WithMany(p => p.States)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasForeignKey(p => p.CountryId);

            modelBuilder.Entity<ContactSPDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<CountDto>().HasNoKey().ToView(null);

        }

        public virtual IQueryable<ContactSPDto> ContactListSP(char? letter,string? search, int page, int pageSize, string sortOrder)
        {
            var letterParam = new SqlParameter("@letter", letter ?? (object)DBNull.Value);
            var searchParam = new SqlParameter("@Search", search ?? (object)DBNull.Value);
            var pageParam = new SqlParameter("@page", page);
            var pageSizeParam = new SqlParameter("@pageSize", pageSize);
            var sortOrderParam = new SqlParameter("@sortOrder", sortOrder);

            return Set<ContactSPDto>().FromSqlRaw("dbo.GetAllContactsSPWithCodeFirst @letter,@search, @page, @pageSize, @sortOrder",letterParam,searchParam, pageParam, pageSizeParam, sortOrderParam);
        }

        public virtual IQueryable<ContactSPDto> GetContactsBasedOnBirthdayMonth(int month)
        {
            var monthParam = new SqlParameter("@month", month);
            return Set<ContactSPDto>().FromSqlRaw("dbo.GetContactsBasedOnBirthdayMonthCF @month", monthParam);
        }
        
        public virtual IQueryable<ContactSPDto> GetContactByState(int state)
        {
            var stateParam = new SqlParameter("@state", state);
            return Set<ContactSPDto>().FromSqlRaw("dbo.GetContactByStateCF @state", stateParam);
        }

        public virtual IQueryable<CountDto> GetContactsCountBasedOnCountry(int countryId)
        {
            var countryParam = new SqlParameter("@countryId", countryId);
            return Set<CountDto>().FromSqlRaw("dbo.GetContactsCountBasedOnCountryCF @countryId", countryParam);
        } 
        public virtual IQueryable<CountDto> GetContactsCountBasedOnGender(string gender)
        {
            var genderParam = new SqlParameter("@gender", gender);
            return Set<CountDto>().FromSqlRaw("dbo.GetContactsCountBasedOnGenderCF @gender", genderParam);
        }



    }
}
