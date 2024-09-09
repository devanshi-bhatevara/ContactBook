using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBookClientApp.ViewModels
{
    public class ContactViewModel
    {
        [Key]
        [DisplayName("Contact Id")]
        public int ContactId { get; set; }

        [Required]
        [StringLength(15)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        [DisplayName("Last Name")]

        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Contact Number")]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [DisplayName("Profile photo")]
        public string? FileName { get; set; } 

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required]
        public bool IsFavourite { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int StateId { get; set; }

        public CountryViewModel Country { get; set; }

        public StateViewModel State { get; set; }
    }
}
