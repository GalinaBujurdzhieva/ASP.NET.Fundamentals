using Microsoft.EntityFrameworkCore;
using SMS.Contracts;
using SMS.Data.Models;
using SMS.Data.Repo;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository repository;

        public CartService(IRepository _repository)
        {
            repository = _repository;
        }
        public IEnumerable<CartViewModel> GetAllProducts(string userId)
        {
            var user = repository.All<User>()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                .ThenInclude(c => c.Products)
                .FirstOrDefault();

            var products = user.Cart.Products
                .Select(p => new CartViewModel
                {
                    ProductName = p.Name,
                    ProductPrice = p.Price.ToString("F2")
                })
                .ToList();

            return products;
        }

        public IEnumerable<CartViewModel> AddProduct(string productId, string userId)
        {
            var user = repository.All<User>()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                .ThenInclude(c => c.Products)
                .FirstOrDefault();

            var product = repository.All<Product>()
                .FirstOrDefault(p => p.Id == productId);

            user.Cart.Products.Add(product);

            List<CartViewModel> usersProducts = new List<CartViewModel>();

            try
            {
                repository.SaveChanges();
            }
            catch (Exception)
            {}
            usersProducts = user.Cart.Products
           .Select(p => new CartViewModel()
           {
               ProductName = p.Name,
               ProductPrice = p.Price.ToString("F2"),
           }).ToList();
            return usersProducts;
        }

        public void Buy(string userId)
        {
            var user = repository.All<User>()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                .ThenInclude(c => c.Products)
                .FirstOrDefault();
            
            user.Cart.Products.Clear();
            repository.SaveChanges();
        }
    }
}