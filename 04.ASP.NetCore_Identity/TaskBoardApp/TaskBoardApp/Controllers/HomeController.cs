using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TaskBoardApp.Data;
using TaskBoardApp.Models;

namespace TaskBoardApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TaskBoardAppDbContext context;

        public HomeController(TaskBoardAppDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            var taskBoards = context.Boards
                .Select(b => b.Name)
                .Distinct();

            var tasksWithCount = new List<HomeBoardViewModel>();

            foreach (var boardName in taskBoards)
            {
                var tasksInBoardCount = context.Tasks.Where(t => t.Board.Name == boardName).Count();
                tasksWithCount.Add(new HomeBoardViewModel()
                {
                    BoardName = boardName,
                    TasksCount = tasksInBoardCount
                });
            }

            int userTasksCount = 0;
            
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                userTasksCount = context.Tasks.Where(t => t.OwnerId == currentUserId).Count();
            }

            var homeModel = new HomeViewModel
            {
                AllTasksCount = context.Tasks.Count(),
                BoardsWithTasksCount = tasksWithCount,
                UserTasksCount = userTasksCount,
            };
            return View(homeModel);
        }
    }
}