using ForumApp.Data;
using ForumApp.Data.Models;
using ForumApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Controllers
{
    public class PostController : Controller
    {
        private readonly ForumAppDbContext _context;
        public PostController(ForumAppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult All()
        {
            var posts = _context.Posts.Select(p => new PostViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
            })
            .ToList();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(PostFormModel model)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content
            };

            _context.Add(post); 
            _context.SaveChanges();

            return RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            var model = new PostFormModel
            {
                Title = post.Title,
                Content = post.Content,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, PostFormModel model)
        {
            var post = _context.Posts.Find(id);
            post.Title = model.Title;   
            post.Content = model.Content;   
     
            _context.SaveChanges();

            return RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            
            _context.Posts.Remove(post);    
            _context.SaveChanges();

            return RedirectToAction("All");
        }
    }
}
