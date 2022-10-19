using System.ComponentModel.DataAnnotations;
using Watchlist.Data.Entities;

namespace Watchlist.Models
{
    public class MovieAddViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Title { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Director { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Range(typeof(decimal), "0.00", "10.00", ConvertValueInInvariantCulture = true)]
        public decimal Rating { get; set; }
        public int GenreId { get; set; }
        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
    }
}
