using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Watchlist.Data.Entities
{
    public class UserMovie
    {
        [Required]
        [Key]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        [Key]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
