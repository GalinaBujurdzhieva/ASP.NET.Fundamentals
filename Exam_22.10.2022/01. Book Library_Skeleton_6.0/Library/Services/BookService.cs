using Library.Contracts;
using Library.Data;
using Library.Data.Entities;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Net;
using System.Security.Claims;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;
        public BookService(LibraryDbContext _libraryDbContext)
        {
            context = _libraryDbContext;
        }

        public async Task<IEnumerable<Category>> GetCategoriesFromDBAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<BookAllViewModel>> GetAllBooksAsync()
        {
            return await context.Books
                .Include(b => b.Category)
                .Select(b => new BookAllViewModel()
                {
                    Id = b.Id,
                    ImageUrl = b.ImageUrl,
                    Title = b.Title,
                    Author = b.Author,
                    Rating = b.Rating,
                    Category = b.Category.Name
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<MyBookViewModel>> GetMyBooksAsync(string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .ThenInclude(aub => aub.Book)
                .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            return user.ApplicationUsersBooks
                .Select(b => new MyBookViewModel
                {
                    Id = b.BookId,
                    ImageUrl = b.Book.ImageUrl,
                    Title = b.Book.Title,
                    Author = b.Book.Author,
                    Description = b.Book.Description,
                    Category = b.Book.Category.Name
                });
        }

        public async Task AddBookAsync(BookAddViewModel model)
        {
            var newBook = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                CategoryId = model.CategoryId
            };

            await context.Books.AddAsync(newBook);
            await context.SaveChangesAsync();
        }

        public async Task AddBookToCollectionAsync(string userId, int bookId)
        {
            var user = await context.Users
           .Where(u => u.Id == userId)
           .Include(u => u.ApplicationUsersBooks)
           .ThenInclude(um => um.Book)
           .ThenInclude(m => m.Category)
           .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var book = await context.Books
                .FirstOrDefaultAsync(m => m.Id == bookId);

            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }

            bool isBookInUserCollection = context.ApplicationUserBooks.Any(x => x.ApplicationUserId == userId && x.BookId == bookId);

            if (!isBookInUserCollection)
            {
                user.ApplicationUsersBooks.Add(new ApplicationUserBook
                {
                    ApplicationUserId = userId,
                    BookId = book.Id,
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveBookFromCollectionAsync(string userId, int bookId)
        {
            var user = await context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.ApplicationUsersBooks)
            .ThenInclude(um => um.Book)
            .ThenInclude(m => m.Category)
            .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var bookInUserCollection = user.ApplicationUsersBooks.FirstOrDefault(aub => aub.BookId == bookId);
            
            if (bookInUserCollection != null)
            {
                context.ApplicationUserBooks.Remove(bookInUserCollection);
                context.SaveChanges();
            }
        }

        
    }
}
