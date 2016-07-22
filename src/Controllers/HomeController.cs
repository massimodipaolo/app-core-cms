using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bom.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult Error(int id)
        {
            switch (id)
            {
                case 400:
                    ViewData["Icon"] = "fa fa-ban text-danger";
                    ViewData["Title"] = "Bad Request";
                    ViewData["Description"] = "Your browser sent a request that this server could not understand.";
                    break;
                case 401:
                    ViewData["Icon"] = "fa fa-ban text-danger";
                    ViewData["Title"] = "Unauthorized";
                    ViewData["Description"] = "Sorry, but the page requires authentication.";
                    break;
                case 403:
                    ViewData["Icon"] = "fa fa-exclamation-circle text-danger";
                    ViewData["Title"] = "Forbidden";
                    ViewData["Description"] = "Sorry, but you don't have permission to access this page.";
                    break;
                case 404:
                    ViewData["Icon"] = "fa fa-exclamation-circle text-danger";
                    ViewData["Title"] = "Page Not Found";
                    ViewData["Description"] = "Sorry, but the page you were looking for can't be found.";
                    break;
                case 500:
                default:
                    ViewData["Icon"] = "fa fa-exclamation-circle text-danger";
                    ViewData["Title"] = "Unexpected Error";
                    ViewData["Description"] = "Well, this is embarrassing. An error occurred while processing your request. Rest assured, this problem has been logged and hamsters have been released to fix the problem.";
                    break;
            }

            return View();
        }
    }
}
