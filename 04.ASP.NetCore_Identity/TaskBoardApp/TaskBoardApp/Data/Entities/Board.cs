using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data.Entities
{
    public class Board
    {
        public Board()
        {
            Tasks = new HashSet<Task>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.BoardNameMaxLength)]
        public string Name { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
