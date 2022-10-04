using System.ComponentModel.DataAnnotations;
using static ForumApp.Data.DataConstants.Post;

namespace ForumApp.Models
{
    public class PostFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = 30)]
        public string Content { get; set; }
    }
}
