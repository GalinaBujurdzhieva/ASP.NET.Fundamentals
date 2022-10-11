using BasicWebServer.Server.Attributes;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using SMS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ICartService cartService;
        public CartsController(Request request, ICartService _cartService) 
            : base(request)
        {
            cartService = _cartService;
        }

        public Response Details()
        {
            var products = cartService.GetAllProducts(User.Id);
            return View(new
            {
                products = products,
                IsAuthenticated = true
            });
        }

        public Response AddProduct(string productId)
        {
            var products = cartService.AddProduct(productId, User.Id);
            
            return View(new 
            {
               products = products,
               IsAuthenticated = true
            }, "/Carts/Details");
        }

        public Response Buy()
        {
            cartService.Buy(User.Id);
            return Redirect("/");
        }

    }
}
