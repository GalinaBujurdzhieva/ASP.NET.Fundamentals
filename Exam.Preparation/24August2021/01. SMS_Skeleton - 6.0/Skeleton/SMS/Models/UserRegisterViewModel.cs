
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class UserRegisterViewModel
    {
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "{0} must be valid e-mail address")]
        public string Email { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "{0} and Password must be the same")]
        public string ConfirmPassword { get; set; }

    }
}
