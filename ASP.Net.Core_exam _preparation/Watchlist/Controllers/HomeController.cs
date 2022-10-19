using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Watchlist.Data.Entities;
using Watchlist.Models;

namespace Watchlist.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<User> signInManager;
        public HomeController(SignInManager<User> _signInManager)
        {
            signInManager = _signInManager;
        }
        public IActionResult Index()
        {
            if (signInManager.IsSignedIn(User)) // User.Identity.IsAuthenticated
            {
                return RedirectToAction("All", "Movies");
            }
            return View();
        }

     
    }
}