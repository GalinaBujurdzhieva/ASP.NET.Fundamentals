namespace TaskBoardApp.Models
{
    public class BoardViewModel
    {
        public BoardViewModel()
        {
            Tasks = new List<TaskViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }
}
