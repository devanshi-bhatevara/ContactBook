using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace ContactBookClientApp.ViewModels
{
    public class StateViewModel
    {
        [Key]
        public int StateId { get; set; }

        [Required]
        [StringLength(25)]
        public string StateName { get; set; }

        [Required]
        public int CountryId { get; set; }

        public CountryViewModel Country { get; set; }

        public virtual ICollection<ContactViewModel> Contacts { get; set; }
    }
}
