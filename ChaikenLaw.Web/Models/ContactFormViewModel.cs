using System.ComponentModel.DataAnnotations;

namespace ChaikenLaw.Web.Models
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}