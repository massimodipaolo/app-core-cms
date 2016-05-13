using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace bom.Controllers
{
    public class HomeController : Controller
    {
        private Models.AppDbContext _context;

        public HomeController(Models.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {   
            return View();
        }       

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Users()
        {   
            return new ObjectResult(_context.Users.ToList());
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
