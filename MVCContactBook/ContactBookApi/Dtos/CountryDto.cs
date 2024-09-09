using ContactBookApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ContactBookApi.Dtos
{
    public class CountryDto
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        [StringLength(25)]
        public string CountryName { get; set; }

        public virtual ICollection<State> States { get; set; }
        public virtual ICollection<ContactBook> Contacts { get; set; }
    }
}
