using Library.Data.Entities;
using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task <IEnumerable<Category>> GetCategoriesFromDBAsync();
        Task<IEnumerable<BookAllViewModel>> GetAllBooksAsync();
        Task<IEnumerable<MyBookViewModel>> GetMyBooksAsync(string userId);

        Task AddBookAsync(BookAddViewModel model);
        Task AddBookToCollectionAsync(string userId, int bookId);
        Task RemoveBookFromCollectionAsync(string userId, int bookId);
    }
}
