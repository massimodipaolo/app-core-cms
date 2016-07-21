using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bom.Controllers
{
    public class RootController : Controller
    {
        public IActionResult Index()
        {            
            var _route = RouteData;
            ViewData["RouteData"] = _route;

            return View();
        }
    }
}