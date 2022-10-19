using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist.Contracts;
using Watchlist.Data.Entities;
using Watchlist.Models;

namespace Watchlist.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;

        public MoviesController(IMovieService _movieService)
        {
            movieService = _movieService;
        }

       public async Task<IActionResult> Add()
        {
            var model = new MovieAddViewModel()
            {
                Genres = await movieService.GetGenresAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MovieAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            try
            {
                await movieService.AddMovieAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not add the movie");
                return View(model);
            }
        }

        public async Task<IActionResult> All()
        {
            var movies = await movieService.GetMoviesAsync();
            return View(movies);
        }

        public async Task<IActionResult> Watched()
        {
            var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            var model = await movieService.GetWatchedMoviesAsync(userId);
            return View("Mine", model) ;
        }

        public async Task<IActionResult> AddToCollection(int movieId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
                await movieService.AddMovieToCollectionAsync(userId, movieId);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> RemoveFromCollection(int movieId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            await movieService.RemoveMovieFromCollectionAsync(userId, movieId);
            return RedirectToAction(nameof(Watched));
        }
    }
}
