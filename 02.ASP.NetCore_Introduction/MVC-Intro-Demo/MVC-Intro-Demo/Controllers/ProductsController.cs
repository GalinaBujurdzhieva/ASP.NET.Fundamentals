using Microsoft.AspNetCore.Mvc;
using MVC_Intro_Demo.Models;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Unicode;

namespace MVC_Intro_Demo.Controllers
{
    public class ProductsController : Controller
    {
        private IEnumerable<ProductViewModel> products = new List<ProductViewModel>()
        {
            new ProductViewModel()
            {
                Id = 1,
                Name = "Cheese",
                Price = 7.00m
            },
              new ProductViewModel()
            {
                Id = 2,
                Name = "Ham",
                Price = 5.50m
            },
                new ProductViewModel()
            {
                Id = 3,
                Name = "Bread",
                Price = 1.50m
            }
        };

        [ActionName("My-Products")]
        public IActionResult All(string keyword)
        {   
            if (keyword != null)
            {
                var neededProducts = products.Where(x => x.Name.ToLower().Contains(keyword.ToLower())).ToList();
                return View(neededProducts);
            }
            return View(products);
        }

        public IActionResult ById(int id)
        {
            var product = products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }
            return View(product);
        }

        public IActionResult AllAsJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return Json(products, options);
        }

        public IActionResult AllAsText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {
                sb.AppendLine(product.ToString());
            }
            return Content(sb.ToString());
        }

        public IActionResult AllAsTextFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {
                sb.AppendLine(product.ToString());
            }
            byte[] bytesFromBuilder = Encoding.UTF8.GetBytes(sb.ToString());
            Response.Headers.Add("content-disposition", "attachment");
            return File(bytesFromBuilder, "text/plain", "products.txt");
        }
    }
}
