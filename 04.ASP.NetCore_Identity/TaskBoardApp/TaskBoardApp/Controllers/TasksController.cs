using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoardApp.Data;
using TaskBoardApp.Data.Entities;
using TaskBoardApp.Models;
using Task = TaskBoardApp.Data.Entities.Task;

namespace TaskBoardApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskBoardAppDbContext context;
        public TasksController(TaskBoardAppDbContext _context)
        {
            context = _context;
        }

        public IActionResult Create()
        {
            var model = new TaskFormViewModel()
            {
                Boards = GetBoards()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(TaskFormViewModel model)
        {
            if (!GetBoards().Any(b => b.Id == model.BoardId))
            {
                ModelState.AddModelError(nameof(model.BoardId), "There is no such board");
            }
            string currentUserId = GetUserId();

            var task = new Data.Entities.Task()
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                BoardId = model.BoardId,
                OwnerId = currentUserId,
            };
            context.Tasks.Add(task);
            context.SaveChanges();
            return RedirectToAction("All", "Boards");
        }

        public IActionResult Details(int id)
        {
            var task = context.Tasks
               .Where(t => t.Id == id)
               .Select(t => new TaskDetailsViewModel()
               {
                   Id = t.Id,
                   Title = t.Title,
                   Description = t.Description,
                   Board = t.Board.Name,
                   Owner = t.Owner.UserName,
                   CreatedOn = t.CreatedOn.ToString("dd/MM/yyyy HH:mm")
               })
               .FirstOrDefault();
            if (task == null)
            {
                return BadRequest();
            }
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            Task task = context.Tasks.Find(id);
            
            if (task == null)
            {
                return BadRequest();
            }
            
            string currentUserId = GetUserId();
            if (task.OwnerId != currentUserId)
            {
                return Unauthorized();
            }

            var model = new TaskFormViewModel
            {
                Title = task.Title,
                Description = task.Description,
                BoardId = task.BoardId,
                Boards = GetBoards()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, TaskFormViewModel model)
        {
            Task task = context.Tasks.Find(id);

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (task.OwnerId != currentUserId)
            {
                return Unauthorized();
            }

            if (!GetBoards().Any(b => b.Id == model.BoardId))
            {
                ModelState.AddModelError(nameof(model.BoardId), "There is no such board");
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.BoardId = model.BoardId;

            context.SaveChanges();
            return RedirectToAction("All", "Boards");
        }

        public IActionResult Delete(int id)
        {
            Task task = context.Tasks.Find(id);

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (task.OwnerId != currentUserId)
            {
                return Unauthorized();
            }

            var model = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(TaskBoardViewModel model)
        {
            var task = context.Tasks.Find(model.Id);
            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            context.Tasks.Remove(task);
            context.SaveChanges();
            return RedirectToAction("All", "Boards");
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private IEnumerable<TaskBoardViewModel> GetBoards()
        {
            return context.Boards
                .Select(b => new TaskBoardViewModel()
                {
                    Id = b.Id,
                    Name = b.Name,
                });
        }
    }
}
