using System.ComponentModel.DataAnnotations;

namespace Library.Data.Entities
{
    public class ApplicationUserBook
    {
        [Required]
        [Key]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Key]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
