using Library.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class BookAddViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Title { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Author { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Description { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Range(typeof(decimal), "0.00", "10.00", ConvertValueInInvariantCulture = true)]
        public decimal Rating { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
