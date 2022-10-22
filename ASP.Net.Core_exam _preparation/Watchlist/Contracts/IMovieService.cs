using Watchlist.Data.Entities;
using Watchlist.Models;

namespace Watchlist.Contracts
{
    public interface IMovieService
    {
        Task AddMovieAsync(MovieAddViewModel model);
        Task<IEnumerable<Genre>> GetGenresAsync(); //ok
        Task<IEnumerable<MovieAllViewModel>> GetMoviesAsync(); //ok
        Task<IEnumerable<MovieAllViewModel>> GetWatchedMoviesAsync(string userId);
        Task AddMovieToCollectionAsync(string userId, int movieId);
        Task RemoveMovieFromCollectionAsync(string userId, int movieId);
    }
}
