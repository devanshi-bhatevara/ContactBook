using System.ComponentModel.DataAnnotations;

namespace ContactBookClientApp.ViewModels
{
    public class CountryViewModel
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        [StringLength(25)]
        public string CountryName { get; set; }

        public virtual ICollection<StateViewModel> States { get; set; }
        public virtual ICollection<ContactViewModel> Contacts { get; set; }
    }
}
