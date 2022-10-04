using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models
{
    public class PostViewModel
    {
        public int Id { get; init; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
