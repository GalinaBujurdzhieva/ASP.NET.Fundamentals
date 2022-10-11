using SMS.Contracts;
using SMS.Data.Models;
using SMS.Data.Repo;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository repository;    
        private readonly IValidationService validationService;

        public ProductService(IRepository _repository, IValidationService _validationService)
        {
            validationService = _validationService; 
            repository = _repository;
        }

        public (bool isCreated, string error) CreateProduct(ProductCreateViewModel model)
        {
            bool isCreated = false;
            string error = string.Empty;

            var (isValid, listOfErrors) = validationService.ValidateModel(model);

            if (!isValid)
            {
                return (isValid, listOfErrors);
            }

            decimal priceResult;
            bool isPriceValid = decimal.TryParse(model.Price, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out priceResult);
            if (!isPriceValid || priceResult < 0.05m || priceResult > 1000m)
            {
                return (isCreated, "Price must be between 0.05 and 1000.");
            }

            var product = new Product
            {
                Name = model.Name,
                Price = priceResult,
            };

            try
            {
                repository.Add(product);
                repository.SaveChanges();
                isCreated = true;

            }
            catch (Exception)
            {
                return (isCreated, "Product can't be added in DB");
            }
            return (isCreated, error);
        }

        public IEnumerable<ProductListViewModel> GetAllProducts()
        {
            return repository.All<Product>().
                Select(p => new ProductListViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductPrice = p.Price.ToString("F2"),
                }).ToList();
        }
    }
}
