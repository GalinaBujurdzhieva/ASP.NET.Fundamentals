using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data.Entities
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.TaskTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DataConstants.TaskDescriptionMaxLength)]
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int BoardId { get; set; }
        public virtual Board Board { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

    }
}
