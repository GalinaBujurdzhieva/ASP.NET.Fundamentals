using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(DataConstants.UserFirstAndLastNameMaxLength)]
        public string FirstName { get; init; }

        [Required]
        [MaxLength(DataConstants.UserFirstAndLastNameMaxLength)]
        public string LastName { get; init; }
    }
}
