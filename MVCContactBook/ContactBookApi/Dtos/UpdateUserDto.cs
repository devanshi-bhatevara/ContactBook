using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBookApi.Dtos
{
    public class UpdateUserDto
    {

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(15)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(15)]
        [DisplayName("Last Name")]
        
        public string LastName { get; set; }

        [Required(ErrorMessage = "Login Id is required")]
        [DisplayName("Login Id")]

        public string LoginId { get; set; }


        [Required(ErrorMessage = "Contact Number is required")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [DisplayName("Contact Number")]

        public string ContactNumber { get; set; }

        public string? FileName { get; set; }

        public byte[]? ImageByte { get; set; }
    }
}
