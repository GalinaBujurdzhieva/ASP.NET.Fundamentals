using Microsoft.EntityFrameworkCore;
using Watchlist.Contracts;
using Watchlist.Data;
using Watchlist.Data.Entities;
using Watchlist.Models;

namespace Watchlist.Services
{
    public class MovieService : IMovieService
    {
        private readonly WatchlistDbContext context;
        public MovieService(WatchlistDbContext _context)
        {
            context = _context;
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await context.Genres.ToListAsync();
        }

        public async Task AddMovieAsync(MovieAddViewModel model)
        {
            var newMovie = new Movie()
            {
                Title = model.Title,
                Director = model.Director,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                GenreId = model.GenreId,
            };
            
            await context.Movies.AddAsync(newMovie);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieAllViewModel>> GetMoviesAsync()
        {
            var dbMovies = await context.Movies
                .Include(m => m.Genre)
                .ToListAsync();

            return dbMovies
                .Select(m => new MovieAllViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    ImageUrl = m.ImageUrl,
                    Director = m.Director,
                    Rating = m.Rating,
                    Genre = m.Genre.Name
                });
        }

        public async Task<IEnumerable<MovieAllViewModel>> GetWatchedMoviesAsync(string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UsersMovies)
                .ThenInclude(um => um.Movie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            return user.UsersMovies
                .Select(m => new MovieAllViewModel()
                { 
                    Id = m.Movie.Id,
                    Title=m.Movie.Title,
                    Director = m.Movie.Director,
                    ImageUrl = m.Movie.ImageUrl,
                    Rating = m.Movie.Rating,
                    Genre = m.Movie.Genre.Name
                });
        }

        public async Task AddMovieToCollectionAsync(string userId, int movieId)
        {
            var user = await context.Users
           .Where(u => u.Id == userId)
           .Include(u => u.UsersMovies)
           .ThenInclude(um => um.Movie)
           .ThenInclude(m => m.Genre)
           .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var movie = await context.Movies
                .FirstOrDefaultAsync(m => m.Id == movieId);
            
            if (movie == null)
            {
                throw new ArgumentException("Movie not found");
            }

            bool isMovieInUserCollection = context.UsersMovies.Any(x => x.UserId == userId && x.MovieId == movieId);
            
            if (!isMovieInUserCollection)
            {
                user.UsersMovies.Add(new UserMovie
                {
                    UserId = userId,
                    MovieId = movie.Id
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveMovieFromCollectionAsync(string userId, int movieId)
        {
            var user = await context.Users
           .Where(u => u.Id == userId)
           .Include(u => u.UsersMovies)
           .ThenInclude(um => um.Movie)
           .ThenInclude(m => m.Genre)
           .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var userMovie = await context.UsersMovies
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (userMovie != null)
            {
                context.UsersMovies.Remove(userMovie);
                await context.SaveChangesAsync();
            }
        }
    }
}
