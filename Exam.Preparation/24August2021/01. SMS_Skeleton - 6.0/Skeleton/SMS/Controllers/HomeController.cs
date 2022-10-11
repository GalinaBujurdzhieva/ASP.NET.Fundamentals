using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using SMS.Data.Repo;
using SMS.Services;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserService userService;
        private readonly ProductService productService;
        public HomeController(Request request, UserService _userService, ProductService _productService)
            : base(request)
        {
            userService = _userService;
            productService = _productService;
        }

        public Response Index()
        {
            if (!User.IsAuthenticated)
            {
                return this.View(new { IsAuthenticated = false });
            } 

            string username = userService.GetUserName(User.Id);
            
            var model = new
            {
                Username = username,
                IsAuthenticated = true,
                Products = productService.GetAllProducts()
            };
            return this.View(model, "/Home/IndexLoggedIn");
        }
    }
}