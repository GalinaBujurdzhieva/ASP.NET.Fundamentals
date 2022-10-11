using BasicWebServer.Server.Attributes;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using SMS.Contracts;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        public UsersController(Request request, IUserService _userService)
            : base(request)
        {
            userService = _userService;
        }

        public Response Register()
        {
            if (User.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View(new { IsAuthenticated = false });
        }

        [HttpPost]
        public Response Register(UserRegisterViewModel model)
        {
            var (isRegistered, errors) = userService.Register(model);
            
            if (isRegistered)
            {
                return Redirect("/Users/Login");
            }
            return View(new { ErrorMessage = errors }, "/Error");
        }

        public Response Login()
        {
            if (User.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View(new { IsAuthenticated = false });
        }

        [HttpPost]
        public Response Login(UserLoginViewModel model)
        {
            Request.Session.Clear();
            
            string userId = userService.Login(model);
            
            if (userId != null)
            {
                SignIn(userId);
                CookieCollection cookies = new CookieCollection();
                cookies.Add(Session.SessionCookieName, Request.Session.Id);
                return Redirect("/");
            }
            return View(new { ErrorMessage = "Incorrect Username or Password" }, "/Error");
        }

        [Authorize]
        public Response Logout()
        {
            SignOut();
            return Redirect("/");
        }
    }
}
