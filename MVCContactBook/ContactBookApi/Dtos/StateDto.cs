using ContactBookApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ContactBookApi.Dtos
{
    public class StateDto
    {

        [Key]
        public int StateId { get; set; }

        [Required]
        [StringLength(25)]
        public string StateName { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public virtual ICollection<ContactBook> Contacts { get; set; }
    }
}
