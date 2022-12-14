using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Watchlist.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string UserName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        [EmailAddress]
        public string Email{ get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "{0} and {1} must be the same")]
        public string ConfirmPassword { get; set; }
    }
}
