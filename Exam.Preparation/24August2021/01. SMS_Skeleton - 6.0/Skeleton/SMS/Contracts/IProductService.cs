using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Contracts
{
    public interface IProductService
    {
        (bool isCreated, string error) CreateProduct(ProductCreateViewModel model);
        IEnumerable<ProductListViewModel> GetAllProducts();

    }
}
