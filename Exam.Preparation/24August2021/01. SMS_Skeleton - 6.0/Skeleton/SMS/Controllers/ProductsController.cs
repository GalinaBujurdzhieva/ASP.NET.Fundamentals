using BasicWebServer.Server.Attributes;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using SMS.Contracts;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        public ProductsController(Request request, IProductService _productService) 
            : base(request)
        {
            productService = _productService;
        }

        public Response Create()
        {
            return View(new { isAuthenticated = true });
        }

        [HttpPost]
        public Response Create(ProductCreateViewModel model)
        {
            var (isCreated, errors) = productService.CreateProduct(model);
            if (isCreated)
            {
                return Redirect("/");
            }
            return View(new { ErrorMessage = errors}, "/Error");
        }


    }
}
