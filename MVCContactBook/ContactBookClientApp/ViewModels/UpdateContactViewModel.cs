using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBookClientApp.ViewModels
{
    public class UpdateContactViewModel
    {
        [Key]
        [DisplayName("Contact Id")]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(15)]
        [DisplayName("First Name")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(15)]
        [DisplayName("Last Name")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [DisplayName("Contact Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(255)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        public string Email { get; set; }

        [DisplayName("Profile photo")]
        public string? FileName { get; set; }

        [Required(ErrorMessage ="Gender is required")]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required]
        [DisplayName("Is Favourite?")]
        public bool IsFavourite { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage ="Country is required")]
        public int CountryId { get; set; }


        [DisplayName("State")]
        [Required(ErrorMessage ="State is required")]
        public int StateId { get; set; }

        [DisplayName("Profile Photo")]

        public IFormFile? File { get; set; }

        public CountryViewModel? Country {  get; set; }

        public StateViewModel? State { get; set; }

        public List<CountryViewModel>? Countries { get; set; }
        public List<StateViewModel>? States { get; set; }

        public byte[]? ImageByte { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? birthDate { get; set; }


        [DisplayName("Do you want to remove image?")]
        public bool RemoveImage { get; set; }
    }
}
