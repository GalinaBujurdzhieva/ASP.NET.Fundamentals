using Library.Contracts;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        public BooksController(IBookService _bookService)
        {
            bookService = _bookService;
        }
        public async Task<IActionResult> All()
        {
            var movies = await bookService.GetAllBooksAsync();    
            return View(movies);
        }

        public async Task<IActionResult> Mine()
        {
            var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            var model = await bookService.GetMyBooksAsync(userId);
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new BookAddViewModel()
            {
                Categories = await bookService.GetCategoriesFromDBAsync(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await bookService.AddBookAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not add the movie");
                return View(model);
            }
        }

        public async Task<IActionResult> AddToCollection(int bookId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
                await bookService.AddBookToCollectionAsync(userId, bookId);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            await bookService.RemoveBookFromCollectionAsync(userId, bookId);
            return RedirectToAction(nameof(Mine));
        }
    }
}
